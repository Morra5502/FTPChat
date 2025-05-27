using System.Net.Sockets;
using System.Text;

namespace FTPChat;

class Client
{
    private const int Port = 83;

    public static void Start()
    {
        Console.Write("Введите IP-адрес сервера: ");
        string serverIp = Console.ReadLine();

        try
        {
            TcpClient client = new TcpClient(serverIp, Port);
            NetworkStream stream = client.GetStream();

            // Поток для чтения сообщений от сервера
            Thread readThread = new Thread(() => ReadMessages(stream));
            readThread.Start();

            Console.WriteLine("Введите сообщения (bye — выход):");
            while (true)
            {
                string messageToSend = Console.ReadLine();
                if (string.IsNullOrEmpty(messageToSend))
                    continue;

                byte[] data = Encoding.UTF8.GetBytes(messageToSend);
                stream.Write(data, 0, data.Length);

                if (messageToSend.ToLower() == "bye")
                    break;
            }

            readThread.Join();
            stream.Close();
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка клиента: {ex.Message}");
        }

        Console.WriteLine("Клиент завершил работу.");
    }

    static void ReadMessages(NetworkStream stream)
    {
        byte[] buffer = new byte[512];
        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Сервер: {received}");

                if (received.Trim().ToLower() == "bye")
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения от сервера: {ex.Message}");
        }
    }
}
