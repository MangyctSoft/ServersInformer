using Npgsql;
using ServersInfo.Interface;
using System;
using System.Data.Common;

namespace ServersInfo.Service
{
    /// <summary>
    /// Сервис для работы с PostgreSQL.
    /// </summary>
    public sealed class DbNpgsqlService : IDbService
    {
        NpgsqlConnection dbConnection;
        public DbNpgsqlService(string connectionString)
        {
            dbConnection = new NpgsqlConnection(connectionString);
        }        
        /// <summary>
        /// Установить соединение с базой данных.
        /// </summary>
        /// <returns>Связь с базой данных.</returns>
        public DbConnection SetConnection()
        {
            return dbConnection;
        }
        /// <summary>
        /// Выполняет запрос.
        /// </summary>
        /// <param name="command">Запрос.</param>
        /// <returns>Результат запроса.</returns>
        public DbCommand GetCommand(string command)
        {
            Console.WriteLine($"{DateTime.Now} :: {dbConnection.Host} :: {command} ");
            return new NpgsqlCommand(command, dbConnection);
        }
        /// <summary>
        /// Выполняет запрос на получение размера базы данных.
        /// </summary>
        /// <param name="dbName">Имя базы данных.</param>
        /// <returns>Результат запроса.</returns>
        public DbCommand GetDbSize(string dbName)
        {
            return GetCommand($"SELECT pg_database_size('{dbName}');");
        }
        /// <summary>
        /// Выполняет запрос на получение списка баз данных кроме временных.
        /// </summary>
        /// <returns>Результат запроса.</returns>
        public DbCommand GetDbList()
        {
            return GetCommand($"SELECT datname FROM pg_database WHERE datistemplate = false;");
        }
    }
}
