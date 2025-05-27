using System;

namespace FTPChat;

public static class ConsoleInterface
{
    public static void Start()
    {
        Console.WriteLine("Выберите режим работы:");
        Console.WriteLine("1 - Запустить сервер");
        Console.WriteLine("2 - Запустить клиент");
        Console.Write("Ваш выбор: ");

        string choice = Console.ReadLine();

        if (choice == "1")
            new Server().Start();
        else if (choice == "2")
            new Client().Start();
        else
            Console.WriteLine("Неверный выбор.");
    }
}