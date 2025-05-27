namespace FTPChat
{
    // FTPCommands.cs
    public static class FTPCommandHandler
    {
        public static string Process(string command)
        {
            string[] parts = command.Split(' ', 2);
            string cmd = parts[0].ToUpperInvariant();
            string arg = parts.Length > 1 ? parts[1] : "";

            return cmd switch
            {
                "USER" => "ОК: имя пользователя принято",
                "PASS" => "ОК: пароль принят",
                "LIST" => "Файлы: file1.txt, file2.txt",
                "RETR" => $"Передача файла: {arg}",
                "STOR" => $"Файл {arg} принят",
                "BYE" => "Сеанс завершён",
                _ => "Неизвестная команда"
            };
        }
    }

}
