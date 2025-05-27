// Этот файл нужен только для запуска. Можно выбрать запуск сервера или клиента.

using FTPChat;
using System;

class Program
{
    static void Main(string[] args)
    {
         ConsoleStart();
    }

    static void ConsoleStart()
    {
        Console.WriteLine("Выберите режим работы:");
        Console.WriteLine("1 - Запустить сервер");
        Console.WriteLine("2 - Запустить клиент");
        Console.Write("Ваш выбор: ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Server.Start();
        }
        else if (choice == "2")
        {
            Client.Start();
        }
        else
        {
            Console.WriteLine("Неверный выбор.");
        }
    }
}
