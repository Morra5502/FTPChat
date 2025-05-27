// Server.cs
using System;
using System.Net;
using System.Net.Sockets;

namespace FTPChat;

public class Server
{
    private const int Port = 83;
    private TcpListener _listener;

    public void Start()
    {
        try
        {
            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();
            Console.WriteLine($"Сервер запущен. Ожидание клиентов на порту {Port}...");

            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Console.WriteLine("Клиент подключён.");

                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сервера: {ex.Message}");
        }
        finally
        {
            _listener?.Stop();
            Console.WriteLine("Сервер завершил работу.");
        }
    }

    private void HandleClient(TcpClient client)
    {
        using NetworkStream stream = client.GetStream();
        var chat = new ChatSession(stream);
        chat.Start();
    }
}