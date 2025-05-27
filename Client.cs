// Client.cs
using System;
using System.Net.Sockets;
using System.Text;
namespace FTPChat;
class Client
{
    private const int Port = 83;

    public static void Main()
    {
        Console.Write("Введите IP-адрес сервера: ");
        string serverIp = Console.ReadLine();

        try
        {
            TcpClient client = new TcpClient(serverIp, Port);
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Введите команды FTP (bye — завершить):");

            while (true)
            {
                Console.Write("> ");
                string command = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(command);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[512];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Сервер: {response}");

                if (command.Trim().ToLower() == "bye")
                    break;
            }

            stream.Close();
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("Клиент завершил работу.");
    }
}
