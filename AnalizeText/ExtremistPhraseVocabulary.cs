using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AnalizeText
{
    /*
     * Класс, представляющий собой словарь экстремистских слов и фраз.
     */ 
    public class ExtremistPhraseVocabulary
    {
        /*
         * phrases - список экстремистских слов и выражений.
         * VOCABULARY_NAME - наименование таблицы в базе данных, которая преставляет собой словарь.
         */
        public List<ExtremistPhrase> phrases;
        private const string VOCABULARY_NAME = "EXTREMIST_PHRASES";

        public ExtremistPhraseVocabulary()
        {
            LoadVocabulary();
        }

        /*
         * Функция загрузки словаря из базы данных.
         */
        private void LoadVocabulary()
        {
            phrases = new List<ExtremistPhrase>();
            List<string> extremistTypes = ExtremistTypes();
            MicrosoftSQLConnection connection = new MicrosoftSQLConnection();
            if (connection.IsOpenConnection())
            {
                SqlDataReader dataReader = connection.SearchTableForName(VOCABULARY_NAME);
                while (dataReader.Read())
                {
                    int phraseType = Convert.ToInt32(dataReader.GetValue(2));
                    phrases.Add(new ExtremistPhrase(Convert.ToString(dataReader.GetValue(1)), 
                        extremistTypes[phraseType - 1]));
                }
            }
            connection.CloseConnection();
        }

        /* 
         * Функция добавления новой экстремистской фразы в словарь.
         * addExtremistPhrase - экстремистская фраза, которую необходимо добавить в словарь.
         */
        public bool AddExtremistPhrase(ExtremistPhrase addExtremistPhrase)
        {
            if (!SearchExtremistPhrase(addExtremistPhrase))
            {
                MicrosoftSQLConnection connection = new MicrosoftSQLConnection();
                bool addElement = false;
                if (connection.IsOpenConnection())
                {
                    string typeID = Convert.ToString(ExtremistTypes().IndexOf(addExtremistPhrase.typePhrase) + 1);
                    string values = "(SELECT 1 + MAX(phrase_id) FROM " + VOCABULARY_NAME +
                        "), N'" + addExtremistPhrase.phrase + "', " + typeID;
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
         * Функция изменения экстремистской фразы в словаре.
         * enterExtremistPhrase - экстремистская фраза, на которую изменяется исходная фраза.
         * updateExtremistPhrase - экстремистская фраза, которая изменяется в словаре.
         */
        public bool UpdateExtremistPhrase(ExtremistPhrase enterExtremistPhrase, ExtremistPhrase updateExtremistPhrase)
        {
            if (SearchExtremistPhrase(updateExtremistPhrase))
            {
                MicrosoftSQLConnection connection = new MicrosoftSQLConnection();
                bool updateElement = false;
                if (connection.IsOpenConnection())
                {
                    string typeID = Convert.ToString(ExtremistTypes().IndexOf(enterExtremistPhrase.typePhrase) + 1); 
                    string set = "phrase = N'" + enterExtremistPhrase.phrase +
                        "', type_id = " + typeID;
                    string IDElement = "phrase_id = " + SearchExtremistPhraseID(updateExtremistPhrase, connection);
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
         * Функция удаления экстремистской фразы из словаря.
         * deleteExtremistPhrase - экстремистская фраза, которая удаляется из словаря.
         */
        public bool DeleteExtremistPhrase(ExtremistPhrase deleteExtremistPhrase)
        {
            if (SearchExtremistPhrase(deleteExtremistPhrase))
            {
                MicrosoftSQLConnection connection = new MicrosoftSQLConnection();
                bool deleteElement = false;
                if (connection.IsOpenConnection())
                {
                    string IDElement = "phrase_id = " + SearchExtremistPhraseID(deleteExtremistPhrase, connection);
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
         * Функция, которая проверяет, существует ли экстремистская фраза в словаре.
         * searchExtremistPhrase - экстремистская фраза, которая проверяется на существование в словаре.
         */
        private bool SearchExtremistPhrase(ExtremistPhrase searchExtremistPhrase)
        {
            return (phrases.IndexOf(searchExtremistPhrase) >= 0);
        }

        /* 
         * Функция нахождения ключа экстремистской фразы в словаре.
         * extremistPhrase - исходная экстремистская фраза.
         * connection - соединение с базой данных.
         */
        private string SearchExtremistPhraseID(ExtremistPhrase extremistPhrase, MicrosoftSQLConnection connection)
        {
            string typeID = Convert.ToString(ExtremistTypes().IndexOf(extremistPhrase.typePhrase) + 1);
            string element = "phrase = N'" + extremistPhrase.phrase +
                "' AND type_id = " + typeID;
            return connection.SearchIDTableElement(VOCABULARY_NAME, element);
        }

        /*
         * Функция вывода списка видов экстремизма из базы данных. 
         */
        public List<string> ExtremistTypes ()
        {
            List<string> extremistTypes = new List<string>();
            MicrosoftSQLConnection connection = new MicrosoftSQLConnection();
            if (connection.IsOpenConnection())
            {
                SqlDataReader dataReader = connection.SearchTableForName("EXTREMIST_TYPES");
                while (dataReader.Read())
                {
                    extremistTypes.Add(Convert.ToString(dataReader.GetValue(1)));
                }
            }
            connection.CloseConnection();
            return extremistTypes;
        }
    }
}
