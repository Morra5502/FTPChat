using System.Net;
using System.Net.Sockets;
using System.Text;
namespace FTPChat;

class Program
{
    private const int Port = 83;
    private static bool isServer = false;
    private static TcpListener listener;
    private static TcpClient client;
    private static NetworkStream stream;
    private static Thread receiveThread;

    static void Main(string[] args)
    {
        Console.WriteLine("Выберите режим работы:");
        Console.WriteLine("1 - Сервер");
        Console.WriteLine("2 - Клиент");
        var choice = Console.ReadLine();

        isServer = choice == "1";

        if (isServer)
        {
            StartServer();
        }
        else
        {
            StartClient();
        }

        Console.WriteLine("Для выхода введите 'bye'");
        while (true)
        {
            var message = Console.ReadLine();
            if (message.ToLower() == "bye")
            {
                break;
            }
            SendMessage(message);
        }

        Stop();
    }

    static void StartServer()
    {
        listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();
        Console.WriteLine($"Сервер запущен на порту {Port}. Ожидание подключений...");

        client = listener.AcceptTcpClient();
        Console.WriteLine("Клиент подключен.");
        stream = client.GetStream();

        receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();
    }

    static void StartClient()
    {
        Console.Write("Введите IP-адрес сервера: ");
        string serverIp = Console.ReadLine();

        client = new TcpClient(serverIp, Port);
        stream = client.GetStream();
        Console.WriteLine("Подключено к серверу.");

        receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();
    }

    static void ReceiveMessages()
    {
        try
        {
            byte[] buffer = new byte[512];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Получено: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка приема: {ex.Message}");
        }
    }

    static void SendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    static void Stop()
    {
        receiveThread?.Abort();
        stream?.Close();
        client?.Close();
        listener?.Stop();
        Console.WriteLine("Приложение завершено.");
    }
}