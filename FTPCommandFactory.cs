// FTPCommandFactory.cs
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FTPChat;

public static class FTPCommandFactory
{
    private static readonly Dictionary<string, ICommand> Commands = new()
    {
        ["USER"] = new UserCommand(),
        ["PASS"] = new PassCommand(),
        ["LIST"] = new ListCommand(),
        ["RETR"] = new RetrCommand(),
        ["STOR"] = new StorCommand(),
        ["BYE"] = new ByeCommand()
    };

    public static string Process(string input)
    {
        string[] parts = input.Split(' ', 2);
        string cmd = parts[0].ToUpperInvariant();
        string arg = parts.Length > 1 ? parts[1] : "";

        return Commands.TryGetValue(cmd, out var command) ? command.Execute(arg) : "Неизвестная команда";
    }
}