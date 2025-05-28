// --- Client.cs ---
using System.Net.Sockets;         // Пространство имён для TCP-клиентов и сетевых потоков.
using System.Text;                // Для преобразования строк в байты и наоборот.

namespace FTPChat;                // Пространство имён для логической организации классов.

class Client                      // Класс, реализующий клиентскую часть TCP-чата.
{
    private const int Port = 83;  // Порт подключения к серверу (должен совпадать с серверным).

    public static void Start()    // Метод запуска клиента.
    {
        //Console.Write("Введите IP-адрес сервера: ");
        //string serverIp = Console.ReadLine();             // Получение IP-адреса от пользователя.

        string serverIp = "127.0.0.1";

        try
        {
            TcpClient client = new TcpClient(serverIp, Port); // Подключение к серверу по IP и порту.
            NetworkStream stream = client.GetStream();        // Получение сетевого потока для общения.

            Thread readThread = new Thread(() => ReadMessages(stream)); // Создание потока для чтения сообщений.
            readThread.Start();

            Console.WriteLine("Введите сообщения (bye — выход):");
            while (true)
            {
                string messageToSend = Console.ReadLine();   // Ввод сообщения пользователем.
                if (string.IsNullOrEmpty(messageToSend)) continue;

                byte[] data = Encoding.UTF8.GetBytes(messageToSend); // Кодирование сообщения в байты.
                stream.Write(data, 0, data.Length);                  // Отправка данных серверу.

                if (messageToSend.ToLower() == "bye") break;        // Завершение при команде "bye".
            }

            readThread.Join();   // Ожидание завершения потока чтения.
            stream.Close();      // Закрытие потока.
            client.Close();      // Закрытие подключения.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка клиента: {ex.Message}");  // Вывод сообщения об ошибке.
        }

        Console.WriteLine("Клиент завершил работу.");
    }

    static void ReadMessages(NetworkStream stream)     // Метод чтения сообщений от сервера.
    {
        byte[] buffer = new byte[512];                 // Буфер для приёма данных.
        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length); // Чтение данных из потока.
                if (bytesRead == 0) break;            // Если ничего не прочитано — сервер отключился.

                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead); // Декодирование сообщения.
                Console.WriteLine($"Сервер: {received}");                        // Вывод на экран.

                if (received.Trim().ToLower() == "bye") break;                 // Завершение при команде.
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения от сервера: {ex.Message}"); // Обработка исключений.
        }
    }
}
