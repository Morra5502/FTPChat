// Client.cs
using System;
using System.Net.Sockets;
using System.Text;

namespace FTPChat;

public class Client
{
    private const int Port = 83;

    public void Start()
    {
        //Console.Write("Введите IP-адрес сервера: ");
        //string serverIp = Console.ReadLine();
        string serverIp = "127.0.0.1";

        try
        {
            using TcpClient client = new TcpClient(serverIp, Port);
            using NetworkStream stream = client.GetStream();
            var chat = new ChatSession(stream);

            Console.WriteLine("Введите сообщения (bye — выход):");
            chat.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка клиента: {ex.Message}");
        }
    }
}