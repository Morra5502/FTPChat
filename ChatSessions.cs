// ChatSession.cs
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FTPChat;

public class ChatSession
{
    private readonly NetworkStream _stream;
    private readonly Thread _readThread;

    public ChatSession(NetworkStream stream)
    {
        _stream = stream;
        _readThread = new Thread(ReadMessages);
    }

    public void Start()
    {
        _readThread.Start();

        while (true)
        {
            string messageToSend = Console.ReadLine();
            if (string.IsNullOrEmpty(messageToSend))
                continue;

            byte[] data = Encoding.UTF8.GetBytes(messageToSend);
            _stream.Write(data, 0, data.Length);

            if (messageToSend.ToLower() == "bye")
                break;
        }

        _readThread.Join();
        _stream.Close();
    }

    private void ReadMessages()
    {
        byte[] buffer = new byte[512];
        try
        {
            while (true)
            {
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Получено: {received}");

                string response = FTPCommandFactory.Process(received);
                if (!string.IsNullOrEmpty(response))
                {
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    _stream.Write(responseBytes, 0, responseBytes.Length);
                }

                if (received.Trim().ToLower() == "bye")
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения: {ex.Message}");
        }
    }
}