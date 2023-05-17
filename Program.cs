using Spectre.Console.Cli;

namespace FanucCLI;

public static class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp<BackupCommand>();
        return app.Run(args);
    }
}
