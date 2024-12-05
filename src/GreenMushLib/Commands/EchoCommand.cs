namespace GreenMushLib.Commands;

internal class TestCommand : IConsoleCommand
{
    public string Name => "echo";

    public string Description => "echoes whatever you tell it to";

    public string Usage => "echo <string>";

    public void Execute(params string[] args)
    {
        if (args.Length < 2)
        {
            CommandManager.PrintToDevConsole(Usage);
            return;
        }

        CommandManager.PrintToDevConsole(args[1].Trim('"'));
    }
}
