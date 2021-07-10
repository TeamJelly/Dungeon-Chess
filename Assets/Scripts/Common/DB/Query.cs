using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.Collections.Generic;

namespace Common.DB
{
    [System.Serializable]
    public class SelectResults<T>
    {
        public T[] results;
    }
    public class Query
    {
        private string dbPath = $"URI=file:{Application.streamingAssetsPath}/db_sqlite/project21.db";
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
            if (sqliteConnection.State != ConnectionState.Open)
            {
                Debug.LogError("DB 연결 실패");
            }
        }

        private string ExecuteDBCommand(string sqlQuery)
        {
            try
            {
                List<Dictionary<string, object>> dict_list = new List<Dictionary<string, object>>();
                SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
                sqliteCommand.CommandText = sqlQuery;

                SqliteDataReader dataReader = sqliteCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    dict_list.Add(new Dictionary<string, object>());
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        var type = dataReader.GetFieldType(i);
                        if (type.Equals(typeof(long)))
                        {
                            dict_list[dict_list.Count - 1].Add(dataReader.GetName(i), dataReader.GetValue(i));
                        }
                        else if (type.Equals(typeof(string)))
                        {
                            dict_list[dict_list.Count - 1].Add(dataReader.GetName(i), $"\"{dataReader.GetValue(i)}\"");
                        }
                    }
                }

                dataReader.Close();
                dataReader = null;
                sqliteCommand.Dispose();
                sqliteCommand = null;

                List<string> result = new List<string>();
                dict_list.ForEach(dict =>
                {
                    result.Add(JSON.DictionaryToJsonString(dict));
                });
                
                return "{\"results\": [" + string.Join(",", result) + "]}";
            }
            catch (SqliteException e)
            {
                Debug.LogError($"SQL 구문이 잘못됐습니다. {sqlQuery} {e}");

                return null;
            }
        }

        private void DisconnectToDB()
        {
            sqliteConnection.Close();
            sqliteConnection = null;
        }

        public SelectResults<T> SelectFrom<T>(string table, string where = null)
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

            DisconnectToDB();
            return JSON.ParseString<SelectResults<T>>(jsonString);
        }
    }
} 