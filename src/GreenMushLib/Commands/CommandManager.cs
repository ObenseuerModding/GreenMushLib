using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Text;

namespace GreenMushLib.Commands;

/// <summary>
/// The static home for all Command related functions
/// </summary>
public static class CommandManager
{
    private static readonly Dictionary<string, IConsoleCommand> CommandRegistry = [];
    internal static DeveloperConsole? LastInConsole { get; private set; }

    static CommandManager()
    {
        Plugin.Log.LogInfo("Command manager is alive");
        RegisterCommand(new TestCommand());
    }

    /// <summary>
    /// Register an IConsoleCommand with the handler
    /// </summary>
    /// <param name="command"></param>
    /// <returns>Returns true on success</returns>
    public static bool RegisterCommand(IConsoleCommand command)
    {

        Plugin.Log.LogInfo($"Registering a command {command.Name}");

        if (CommandRegistry.TryGetValue(command.Name, out var exCmd))
        {
            return false;
        }

        CommandRegistry[command.Name] = command;
        return true;
    }

    [HarmonyPatch(typeof(DeveloperConsole), nameof(DeveloperConsole.HandleInput))]
    [HarmonyPrefix]
    internal static bool HandleInput_Prefix(DeveloperConsole __instance, string input)
    {
        LastInConsole = __instance;
        //Plugin.Log.LogInfo($"Running HandleCommand with argument {input}");

        if (LastInConsole == null)
            return true;

        //Block bad input
        if (input == "")
            return false;

        string[] args = input.SplitOutsideQuotes(' ');
        string cmdName = args[0] ?? string.Empty;

        //Plugin.Log.LogMessage($"Command name: {cmdName}");

        if (cmdName.ToLowerInvariant() == "ext_help")
        {
            ShowHelp();
            return false;
        }

        if (CommandRegistry.TryGetValue(cmdName, out var command))
        {
            try
            {
                command.Execute(args);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"\n\nEncountered an exception while running a command:\n  Command: {cmdName}\n  Args: {input}\n  Ex: {ex}");
            }

            return false;

        }

        return true;
    }

    private static void ShowHelp()
    {
        StringBuilder sb = new("--- Green Mushroom Lib Extended Console ---\n\n");

        foreach (IConsoleCommand command in CommandRegistry.Values)
        {
            sb.Append("  ");
            sb.AppendLine(command.Name);
            sb.AppendLine(command.Description);
        }

        PrintToDevConsole(sb);
    }

    internal static void PrintToDevConsole(string message)
    => LastInConsole?.Print(message);

    /// <summary>
    /// Prints a message to the dev console
    /// </summary>
    /// <param name="message"></param>
    public static void PrintToDevConsole(object message)
    => PrintToDevConsole(message.ToString());
}
