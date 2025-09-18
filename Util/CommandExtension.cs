using System.CommandLine;
using System.ComponentModel.Design;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Util;

public static class CommandExtension
{
    public static RootCommand AddCommand(this RootCommand command, IHost host, Assembly asm)
    {
        IEnumerable<ICommand> subs = asm.GetTypes()
            .Where(t =>
                typeof(ICommand).IsAssignableFrom(t) &&
                t is { IsClass: true, IsAbstract: false }
            )
            .Select(Activator.CreateInstance)
            .OfType<ICommand>();

        return command.AddCommand(host, subs.ToArray());
    }
    
    private static RootCommand AddCommand(this RootCommand command, IHost host, params ICommand[] subs)
    {
        foreach (ICommand subCommand in subs)
        {
            command.Subcommands.Add(subCommand.ToCommand(host));
        }

        return command;
    }

    private static Command ToCommand(this ICommand sub, IHost host)
    {
        Command subCommand = sub.Command;
        foreach (Option opt in sub.Options)
        {
           subCommand.Options.Add(opt);
        }
        subCommand.SetAction(o => sub.InvokeAsync(o, host.Services.CreateScope()));
        return subCommand;
    }
}