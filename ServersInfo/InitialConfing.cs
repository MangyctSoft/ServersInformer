﻿using ServersInfo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ServersInfo
{
    /// <summary>
    /// Класс для проверки на наличие конфигурационного файла.
    /// </summary>
    /// <remarks>В случаи отсутствия конфигурационного файла, создает его.</remarks>
    public static class InitialConfing
    {
        /// <summary>
        /// Время ожидания до обновления (миллисекунды).
        /// </summary>       
        public static int TimeOut { get; private set; }
        /// <summary>
        /// Имя клиент JSON-файла.
        /// </summary>
        public static string ClientJson { get; private set; }
        /// <summary>
        /// Идентификатор документа.
        /// </summary>
        public static string SpreadsheetId { get; private set; }
        /// <summary>
        /// Список серверов.
        /// </summary>
        public static IEnumerable<Server> Servers { get; private set; }      
        /// <summary>
        /// Открыть или создать конфигурационный файл.
        /// </summary>
        public static void Create()
        {
            try
            {
                string path = Path.Combine(Environment.CurrentDirectory, "config.xml");
                if (File.Exists(path))
                {
                    OpenConfig(path);
                }
                else
                {
                    CreateConfig(path);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Создание, заполнение данными по умолчанию
        /// и сериализация конфигурационного файла.
        /// </summary>
        /// <param name="xmlFile">Путь к файлу.</param>
        private static void CreateConfig(string path)
        {
            var server1 = new Server
            {
                Host        = "localhost",
                User        = "postgres",
                Password    = "password",
                Port        = "5432",
                Size        = 20d
            };
            var serverList = new List<Server>
            {
                server1
            };
            var configXML = new ConfigXML
            {
                TimeOut         = 10000,
                ClientJson      = "client.json",
                SpreadsheetId   = "Вставьте идентификатор документа",
                Servers         = serverList
            };

            var xs = new XmlSerializer(typeof(ConfigXML));           
            using (FileStream stream = File.Create(path))
            {
                xs.Serialize(stream, configXML);
                SetPropeties(configXML);
            }
        }
        /// <summary>
        /// Открытие и десериализация конфигурационного файла.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        private static void OpenConfig(string path)
        {
            using (FileStream xmlLoad = File.Open(path, FileMode.Open))
            {
                var xs          = new XmlSerializer(typeof(ConfigXML));
                var configXML   = (ConfigXML)xs.Deserialize(xmlLoad);
                SetPropeties(configXML);
            }
        }
        /// <summary>
        /// Установка свойств для работы с Google Tables.
        /// </summary>
        /// <param name="configXML"><see cref="ConfigXML"/></param>
        private static void SetPropeties(ConfigXML configXML)
        {
            TimeOut         = configXML.TimeOut;
            ClientJson      = configXML.ClientJson;
            SpreadsheetId   = configXML.SpreadsheetId;
            Servers         = configXML.Servers;
        }        
    }
}
