// --- Server.cs ---
using System.Net;                  // Пространство имён для IP-адресов и других сетевых функций.
using System.Net.Sockets;         // Пространство имён для работы с TCP/UDP сокетами.
using System.Text;                // Для кодировки текста в байты и обратно.

namespace FTPChat;                // Пространство имён для логического объединения классов приложения.

class Server                      // Класс, реализующий серверную часть чата.
{
    private const int Port = 83;  // Порт, на котором будет слушать сервер.
    private static TcpListener listener;  // Объект, принимающий входящие TCP-соединения.

    public static void Start()    // Метод запуска сервера.
    {
        try
        {
            listener = new TcpListener(IPAddress.Any, Port);  // Привязка к любому IP на указанном порту.
            listener.Start();                                // Запуск прослушивания входящих соединений.
            Console.WriteLine($"Сервер запущен...");

            TcpClient client = listener.AcceptTcpClient(); // Ожидание подключения клиента (блокирующий вызов).
            Console.WriteLine("Клиент подключён.");

            NetworkStream stream = client.GetStream();     // Получаем поток данных от клиента.

            Thread readThread = new Thread(() => ReadMessages(stream)); // Запуск потока чтения сообщений.
            readThread.Start();

            while (true)                                    // Главный цикл отправки сообщений.
            {
                string messageToSend = Console.ReadLine();  // Ввод сообщения с клавиатуры.
                if (string.IsNullOrEmpty(messageToSend)) continue;

                byte[] data = Encoding.UTF8.GetBytes(messageToSend); // Преобразуем текст в байты.
                stream.Write(data, 0, data.Length);                  // Отправка по сети.

                if (messageToSend.ToLower() == "bye") break;         // Завершение при команде "bye".
            }

            readThread.Join();       // Дождаться завершения потока чтения.
            stream.Close();          // Закрытие сетевого потока.
            client.Close();          // Закрытие клиента.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сервера: {ex.Message}");  // Обработка ошибок.
        }
        finally
        {
            listener?.Stop();       // Остановка прослушивания порта.
            Console.WriteLine("Сервер завершил работу.");
        }
    }

    static void ReadMessages(NetworkStream stream)         // Метод чтения сообщений клиента.
    {
        byte[] buffer = new byte[512];                     // Буфер для чтения.
        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);  // Чтение байтов из потока.
                if (bytesRead == 0) break; // Клиент отключился.

                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);  // Преобразование байтов в строку.
                Console.WriteLine($"Клиент: {received}");                         // Вывод сообщения.

                if (received.Trim().ToLower() == "bye") break;                   // Завершение по команде.
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения от клиента: {ex.Message}");       // Обработка ошибок.
        }
    }
}
