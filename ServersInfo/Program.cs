﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ServersInfo.BL;
using ServersInfo.Interface;
using ServersInfo.Models;

namespace ServersInfo
{
    /*
     * Конфигурационный файл создается автоматически при первом запуске приложения,
     * в корневой папке программы.
     * 
     * Далее необходимо скачать настройки для доступа к Google Sheets (client_secret.json),
     * закинуть в корень программы и прописать его в конфигурационном файле в секции <ClientJson>.
     * 
     * Открыть Google Sheets https://docs.google.com/spreadsheets/d/ [1tOC2Hz2kZ-######-co5JIxA9-#########] /edit#gid=0
     * и скопировать из адресной строки id документа формата - 1tOC2Hz2kZ-######-co5JIxA9-#########,
     * прописать этот id в конфигурационном файле в секции <SpreadsheetId>.
     * 
     * В секцию <Servers> добавить данный о серверах в формате 
     * <Server Host="_hostname_" Username="_username_" Password="_password_" Port="_5432_" Size="_размер диска_" />
     */
    class Program
    {
        static IDatabaseHelper dbHelper;            // Хелпер для работы с базами данных
        static IGoogleSheetsHelper googleSheets;    // Хелпер для работы с Google Sheets

        static void Main(string[] args)
        {
            Console.Write($"{ DateTime.Now} :: Проверка JSON файла ...");
            while (!Initialization())
            {
                Console.Write($"\n{ DateTime.Now} :: Отсутствует файл JSON для связи с Google Sheets.\n" +
                    "Отредактируйте конфигурационный файл и нажмите ENTER.");
                Console.ReadLine();
            }
            Console.Write("Успешно.\n");
            Console.Write($"{ DateTime.Now} :: Проверка SpreadsheetId ...");
            while (!googleSheets.CheckExistSpreadsheetId())
            {
                Console.Write($"\n{ DateTime.Now} :: Не правильный SpreadsheetId.\n" +
                    "Отредактируйте конфигурационный файл и нажмите ENTER.");
                Console.ReadLine();
                Initialization();
            }
            Console.Write("Успешно.\n");

            GoogleSheetsUpdateAsync();

            Console.ReadLine();            
        }
        /// <summary>
        /// Асинхронное обновление документа Google Sheet
        /// </summary>
        static async void GoogleSheetsUpdateAsync()
        {
            await Task.Run(() => GoogleSheetsUpdate());
        }
        /// <summary>
        /// Инициализация и проверка параметров
        /// </summary>
        /// <returns></returns>
        static bool Initialization()
        {
            InitialApp.Run();
            dbHelper    = new DatabaseHelper();
            // Проверяем существование JSON-файла, прописанного в конфигурационном файле
            string path = Path.Combine(Environment.CurrentDirectory, InitialApp.ClientJson);
            if (File.Exists(path))
            {
                googleSheets = new GoogleSheetsHelper(InitialApp.ClientJson, InitialApp.SpreadsheetId);
                return true;
            }
            else
            {
                return false;
            }            
        }    
        /// <summary>
        /// Обновление документа Google Sheets
        /// </summary>
        static void GoogleSheetsUpdate()
        {
            // Повторять каждые  InitialApp.TimeOut
            while (true)
            {
                // Обходим список серверов из конфигурационного файла
                foreach (var item in InitialApp.Servers)
                {
                    var list = dbHelper.GetDatabaseInfo(item.ToString());
                    // Если на сервере есть БД, добавить информацию в Google Sheets
                    if (list.Count > 0)
                    {
                        var table = new List<TableEntry>();
                        var sheet = new TableEntry()
                        {
                            ServerHost      = item.Host,
                            ServerDbInfos   = list
                        };
                        table.Add(sheet);
                        googleSheets.CreateEntryTable(table, item.Size);                        
                    }                    
                }
                //Console.WriteLine($"{ DateTime.Now} :: Документ обновлен.");
                Console.WriteLine($"{ DateTime.Now} :: Ожидание...");
                Thread.Sleep(InitialApp.TimeOut);
            }
        }

    }
    
}
