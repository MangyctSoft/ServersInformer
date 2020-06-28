using System.Data.Common;

namespace ServersInfo.Interface
{
    /// <summary>
    /// Интерфейс для работы с разными провайдерами баз данных.
    /// </summary>
    public interface IDbService
    {
        /// <summary>
        /// Устанавливает связь с бд.
        /// </summary>
        /// <returns></returns>
        DbConnection SetConnection();
        /// <summary>
        /// Выполняет запрос.
        /// </summary>
        /// <param name="command">Запрос.</param>
        /// <returns></returns>
        DbCommand GetCommand(string command);
        /// <summary>
        /// Получает размер базы данных.
        /// </summary>
        /// <param name="dbName">Имя базы данных.</param>
        /// <returns></returns>
        DbCommand GetDbSize(string dbName);
        /// <summary>
        /// Получает список баз данных.
        /// </summary>
        /// <returns></returns>
        DbCommand GetDbList();
    }
}
