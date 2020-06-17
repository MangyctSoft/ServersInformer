using Npgsql;
using ServersInfo.Interface;
using ServersInfo.Models;
using System;
using System.Collections.Generic;

namespace ServersInfo.BL
{
    /// <summary>
    /// Класс для работы с БД.
    /// </summary>
    public sealed class DatabaseHelper : IDatabaseHelper
    {
        /// <summary>
        /// Получает информацию по каждой БД.
        /// </summary>
        /// <param name="connectionString">Строка подключения к серверу.</param>
        /// <returns>Возвращает сведения по всем базам данных с сервера.</returns>
        public IEnumerable<ServerDbInfo> GetDatabaseInfo(string connectionString)
        {
            var result = new List<ServerDbInfo>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                var host = GetHostServer(connectionString);
                Console.Write($"{DateTime.Now} :: Подключение к серверу {host} ... ");
                try
                {
                    conn.Open();
                }
                catch (Exception)
                {
                    Console.Write("Ошибка!\n");
                    return result;
                }
                Console.Write("Подключен.\n");
                string [] dbNames = GetDatabases(conn);
                for (int i = 0; i < dbNames.Length; i++)
                {
                    // Запросить размер БД
                    using (var command = new NpgsqlCommand($"SELECT pg_database_size('{dbNames[i]}');", conn))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var item = new ServerDbInfo
                            {
                                DatabaseName = dbNames[i],
                                DatabaseSize = SetGigabyteSize(reader.GetInt32(0)),
                                DateUpdated = DateTime.Now.ToString()
                            };
                            result.Add(item);
                        }
                        reader.Close();
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Получить список всех БД с сервера, кроме временных.
        /// </summary>
        /// <param name="conn">Сервер.</param>
        /// <returns>Возвращает массив имен баз данных.</returns>
        private string[] GetDatabases(NpgsqlConnection conn)
        {
            var result = new List<string>();
            // Запросить список всех БД, кроме временных
            using (var command = new NpgsqlCommand($"SELECT datname FROM pg_database WHERE datistemplate = false;", conn))
            {
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var database = reader.GetValue(0).ToString();
                    result.Add(database);
                }
                reader.Close();
            }
            return result.ToArray();
        }
        /// <summary>
        /// Перевод размер базы данных в Гб.
        /// </summary>
        /// <param name="size">Размер базы данных.</param>
        /// <returns>Возвращает размер базы данных в строковом виде.</returns>
        private string SetGigabyteSize(double size)
        {
            return (size / 1000000000).ToString("#.###");
        }
        /// <summary>
        /// Получить Host сервера.
        /// </summary>
        /// <param name="connectionString">Строка подключения.</param>
        /// <returns>Возвращает имя сервера.</returns>
        private string GetHostServer(string connectionString)
        {
            var arrayOption = connectionString.Split(";");
            foreach(var item in arrayOption)
            {
                if (item.Contains("Host"))
                {
                    var hostName = item.Split("=");
                    return hostName[1];
                }
            }
            return @"Method GetHostServer error?:\";
        }
    }
}
