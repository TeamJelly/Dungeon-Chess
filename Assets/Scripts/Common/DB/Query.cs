using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.Collections.Generic;

namespace Common.DB
{
    public class Query
    {
        private string dbPath = $"URI=file:{Application.streamingAssetsPath}/project21.db";
        private SqliteConnection sqliteConnection = null;
        private static Query instance = null;
        public static Query Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new Query();
                }
                return instance;
            }
        }

        private Query() {}

        private void ConnectToDB()
        {
            sqliteConnection = new SqliteConnection(dbPath);
            sqliteConnection.Open();
            if (sqliteConnection.State == ConnectionState.Open)
            {
                Debug.Log("DB 연결 성공");
            }
            else
            {
                Debug.Log("DB 연결 실패");
            }
        }

        private string ExecuteDBCommand(string sqlQuery)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
                sqliteCommand.CommandText = sqlQuery;

                SqliteDataReader dataReader = sqliteCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        var type = dataReader.GetFieldType(i);
                        if (type.Equals(typeof(long)))
                        {
                            dict.Add(dataReader.GetName(i), dataReader.GetValue(i));
                        }
                        else if (type.Equals(typeof(string)))
                        {
                            dict.Add(dataReader.GetName(i), $"\"{dataReader.GetValue(i)}\"");
                        }
                    }
                }

                dataReader.Close();
                dataReader = null;
                sqliteCommand.Dispose();
                sqliteCommand = null;
                sqliteConnection.Close();
                sqliteConnection = null;

                string result = JSON.DictionaryToJsonString(dict);
                return result;
            }
            catch (SqliteException e)
            {
                Debug.LogError($"SQL 구문이 잘못됐습니다. {sqlQuery} {e}");

                return null;
            }
        }

        public T SelectFrom<T>(string table, string where = null)
        {
            string jsonString = null;
            ConnectToDB();

            if (where is null)
            {
                jsonString = ExecuteDBCommand($"SELECT * FROM {table}");
            }
            else
            {
                jsonString = ExecuteDBCommand($"SELECT * FROM {table} WHERE {where}");
            }

            return JSON.ParseString<T>(jsonString);
        }
    }
}