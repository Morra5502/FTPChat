// Server.cs
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace FTPChat;

class Server
{
    private const int Port = 83;

    public static void Main()
    {
        TcpListener listener = null;
        try
        {
            Console.WriteLine("FTP-Сервер запускается...");
            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();

            Console.WriteLine($"Ожидание подключения на порту {Port}...");
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Клиент подключён.");

            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[512];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string command = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Получено: {command}");

                string response = FTPCommandHandler.Process(command.Trim());
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);

                if (command.Trim().ToLower() == "bye")
                    break;
            }

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            listener?.Stop();
        }

        Console.WriteLine("Сервер завершил работу.");
    }
}
