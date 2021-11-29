using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AnalizeText
{
    /*
     * Класс, предназначенный для подключения к базе данных.
     */ 
    public class MicrosoftSQLConnection
    {
        /*
         * SQLconnection - соединение с базой данных.
         * CONNECTION_STRING - строка соединения с базой данных.
         */
        private SqlConnection SQLconnection;
        private const string CONNECTION_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;
            AttachDbFilename=""|DataDirectory|\ExtremistDatabase.mdf"";Integrated Security=True";

        public MicrosoftSQLConnection()
        {
            SQLconnection = new SqlConnection(CONNECTION_STRING);
        }

        /*
         * Функция, проверяющая подключение соединения. Если ранее соединение не было подключено,
         *     то оно происходит в данной функции.
         */
        public bool IsOpenConnection()
        {
            if (SQLconnection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    SQLconnection.Open();
                    return true;
                }
                catch
                {
                    SQLconnection.Close();
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /*
         * Функция добавления данных, представленных в виде строки, в таблицу.
         * tableName - имя таблицы, в которую добавляются данные.
         * values - данные, представленные в виде строки.
         */
        public bool AddTableElement(string tableName, string values)
        {
            string requestText = "INSERT INTO " + tableName + " VALUES (" + values + ");";
            SqlCommand command = new SqlCommand(requestText, SQLconnection);
            return (command.ExecuteNonQuery() == 1);
        }

        /*
         * Функция изменения ранее добавленных данных в таблице.
         * tableName - имя таблицы, в которой производятся изменения.
         * setParameters - параметры данных, на которые меняются исходные данные.
         * IDElement - ключ строки, в которой изменяются данные.
         */
        public bool UpdateTableElement(string tableName, string setParameters, string IDElement)
        {
            string requestText = "UPDATE " + tableName + " SET " + setParameters + " WHERE " + IDElement + ";";
            SqlCommand command = new SqlCommand(requestText, SQLconnection);
            return (command.ExecuteNonQuery() == 1);
        }

        /*
         * Функция удаления данных из таблицы.
         * tableName - имя таблицы, в которой производится удаление.
         * IDElement - ключ строки, которая удаляется из таблицы.
         */
        public bool DeleteTableElement(string tableName, string IDElement)
        {
            string requestText = "DELETE FROM " + tableName + " WHERE " + IDElement + ";";
            SqlCommand command = new SqlCommand(requestText, SQLconnection);
            return (command.ExecuteNonQuery() == 1);
        }

        /*
         * Функция извлечения данных из таблицы.
         * tableName - имя таблицы, из которой производится извлечение данных.
         */
        public SqlDataReader SearchTableForName(string tableName)
        {
            string requestText = "SELECT * FROM " + tableName + ";";
            return new SqlCommand(requestText, SQLconnection).ExecuteReader();
        }

        /*
         * Функция нахождения ключа строки данных по ее другим значениям.
         * tableName - имя таблицы, в которой производится поиск.
         * element - данные строки, которую необходимо найти.
         * result - номер искомого объекта.
         */
        public string SearchIDTableElement(string tableName, string element)
        {
            string requestText = "SELECT * FROM " + tableName + " WHERE " + element + ";";
            SqlDataReader dataReader = new SqlCommand(requestText, SQLconnection).ExecuteReader();
            dataReader.Read();
            string result = Convert.ToString(dataReader.GetValue(0));
            dataReader.Close();
            return result;
        }
        
        /*
         * Функция закрытия соединения с базой данных.
         */
        public void CloseConnection()
        {
            if (SQLconnection.State != System.Data.ConnectionState.Closed)
            {
                SQLconnection.Close();
            }
        }
    }
}
