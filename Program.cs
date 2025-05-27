namespace FTPChat;

class Program
{
    static void Main(string[] args)
    {
        if (args.Contains("--server"))
        {
            Server.Start();
        }
        else if (args.Contains("--client"))
        {
            
            string ip = args.Length > 1 ? args[1] : "127.0.0.1";
            Client.Start(ip);
            
        }
    }
}


