using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AnalizeText
{
    /*
     * Класс для хранения словаря экстремистских текстов.
     */
    public class ExtremistTextVocabulary
    {
        /*
         * texts - список экстремистских тектов, взятый из словаря (базы данных).
         * VOCABULARY_NAME - наименование таблицы в базе данных, которая преставляет собой словарь.
         */
        public List<ExtremistText> texts;
        private const string VOCABULARY_NAME = "EXTREMIST_TEXTS";

        public ExtremistTextVocabulary()
        {
            LoadVocabulary();
        }

        /* 
         * Функция загрузки словаря из базы данных.
         */ 
        private void LoadVocabulary()
        {
            texts = new List<ExtremistText>();
            MicrosoftSQLConnection connection = new MicrosoftSQLConnection();
            if (connection.IsOpenConnection())
            {
                SqlDataReader dataReader = connection.SearchTableForName(VOCABULARY_NAME);
                while (dataReader.Read())
                {
                    texts.Add(new ExtremistText(Convert.ToString(dataReader.GetValue(1)),
                        Convert.ToString(dataReader.GetValue(2)), Convert.ToString(dataReader.GetValue(3))));
                }
            }
            connection.CloseConnection();
        }

        /* 
         * Функция добавления нового экстремистского текста в словарь.
         * addExtremistText - экстремистский текст, который необходимо добавить в словарь.
         */
        public bool AddExtremistText(ExtremistText addExtremistText)
        {
            if (!SearchExtremistText(addExtremistText))
            {
                MicrosoftSQLConnection connection = new MicrosoftSQLConnection();
                bool addElement = false;
                if (connection.IsOpenConnection())
                {
                    string values = "(SELECT 1 + MAX(text_id) FROM " + VOCABULARY_NAME + 
                        "), N'" + addExtremistText.startWords + "', N'" + addExtremistText.endWords + 
                        "', N'" + addExtremistText.fullText + "'";
                    addElement = connection.AddTableElement(VOCABULARY_NAME, values);
                }
                connection.CloseConnection();
                LoadVocabulary();
                return addElement;
            }
            else
            {
                return false;
            }
        }

        /* 
         * Функция изменения экстремистского текста в словаре.
         * enterExtremistText - экстремистский текст, на который изменяется исходный текст.
         * updateExtremistText - экстремистский текст, который изменяется в словаре.
         */
        public bool UpdateExtremistText(ExtremistText enterExtremistText, ExtremistText updateExtremistText)
        {
            if (SearchExtremistText(updateExtremistText))
            {
                MicrosoftSQLConnection connection = new MicrosoftSQLConnection();
                bool updateElement = false;
                if (connection.IsOpenConnection())
                {
                    string set = "start_text = N'" + enterExtremistText.startWords +
                        "', finish_text = N'" + enterExtremistText.endWords +
                        "', full_text = N'" + enterExtremistText.fullText + "'";
                    string IDElement = "text_id = " + SearchExtremistTextID(updateExtremistText, connection);
                    updateElement = connection.UpdateTableElement(VOCABULARY_NAME, set, IDElement);
                }
                connection.CloseConnection();
                LoadVocabulary();
                return updateElement;
            }
            else
            {
                return false;
            }
        }

        /* 
         * Функция удаления экстремистского текста из словаря.
         * deleteExtremistText - экстремистский текст, который удаляется из словаря.
         */
        public bool DeleteExtremistText(ExtremistText deleteExtremistText)
        {
            if (SearchExtremistText(deleteExtremistText))
            {
                MicrosoftSQLConnection connection = new MicrosoftSQLConnection();
                bool deleteElement = false;
                if (connection.IsOpenConnection())
                {
                    string IDElement = "text_id = " + SearchExtremistTextID(deleteExtremistText, connection);
                    deleteElement = connection.DeleteTableElement(VOCABULARY_NAME, IDElement);
                }
                connection.CloseConnection();
                LoadVocabulary();
                return deleteElement;
            }
            else
            {
                return false;
            }
        }

        /* 
         * Функция, которая проверяет, существует ли экстремистский текст в словаре.
         * searchExtremistText - экстремистский текст, который проверяется на существование в словаре.
         */
        private bool SearchExtremistText(ExtremistText searchExtremistText)
        {
            return (texts.IndexOf(searchExtremistText) >= 0);
        }

        /* 
         * Функция нахождения ключа экстремистского текста в словаре.
         * extremistText - исходный экстремистский текст.
         * connection - соединение с базой данных.
         */
        private string SearchExtremistTextID(ExtremistText extremistText, MicrosoftSQLConnection connection)
        {
            string element = "start_text = N'" + extremistText.startWords +
                "' AND finish_text = N'" + extremistText.endWords +
                "' AND full_text = N'" + extremistText.fullText + "'";
            return connection.SearchIDTableElement(VOCABULARY_NAME, element);
        }
    }
}