namespace FTPChat;

public class UserCommand : ICommand
{
    public string Execute(string argument) => "ОК: имя пользователя принято";
}

public class PassCommand : ICommand
{
    public string Execute(string argument) => "ОК: пароль принят";
}

public class ListCommand : ICommand
{
    public string Execute(string argument) => "Файлы: file1.txt, file2.txt";
}

public class RetrCommand : ICommand
{
    public string Execute(string argument) => $"Передача файла: {argument}";
}

public class StorCommand : ICommand
{
    public string Execute(string argument) => $"Файл {argument} принят";
}

public class ByeCommand : ICommand
{
    public string Execute(string argument) => "Сеанс завершён";
}