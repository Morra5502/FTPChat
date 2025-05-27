// ICommand.cs
namespace FTPChat;

public interface ICommand
{
    string Execute(string argument);
}