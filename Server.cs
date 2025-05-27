using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FTPChat;

class Server
{
    private const int Port = 83;
    private static TcpListener listener;

    public static void Start()
    {
        try
        {
            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine($"Сервер запущен. Ожидание клиента на порту {Port}...");

            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Клиент подключён.");

            NetworkStream stream = client.GetStream();

            // Запускаем поток для чтения сообщений от клиента
            Thread readThread = new Thread(() => ReadMessages(stream));
            readThread.Start();

            // Главный поток — для отправки сообщений клиенту
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
            Console.WriteLine($"Ошибка сервера: {ex.Message}");
        }
        finally
        {
            listener?.Stop();
            Console.WriteLine("Сервер завершил работу.");
        }
    }

    static void ReadMessages(NetworkStream stream)
    {
        byte[] buffer = new byte[512];
        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // Клиент отключился

                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Клиент: {received}");

                if (received.Trim().ToLower() == "bye")
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения от клиента: {ex.Message}");
        }
    }
}
