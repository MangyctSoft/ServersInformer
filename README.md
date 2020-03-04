# ServersInformer
Работа с Google Tables.
Приложение, которое определяет размер баз данных PostgreSQL на диске.
Результат работы приложения создание/обновление документа в Google Tables.
Конфигурационный файл создается автоматически при первом запуске приложения,
в корневой папке программы.
Далее необходимо скачать настройки для доступа к Google Sheets (client_secret.json),
закинуть в корень программы и прописать его в конфигурационном файле в секции <ClientJson>.
Открыть Google Sheets https://docs.google.com/spreadsheets/d/ [1tOC2Hz2kZ-######-co5JIxA9-#########] /edit#gid=0
и скопировать из адресной строки id документа формата - 1tOC2Hz2kZ-######-co5JIxA9-#########,
прописать этот id в конфигурационном файле в секции <SpreadsheetId>.
В секцию <Servers> добавить данный о серверах в формате 
<Server Host="_hostname_" Username="_username_" Password="_password_" Port="_5432_" Size="_размер диска_" />
