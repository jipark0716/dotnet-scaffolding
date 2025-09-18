using System.CommandLine;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Util;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface ICommand
{
    IEnumerable<Option> Options { get; }
    Command Command { get; }
    Task InvokeAsync(ParseResult parseResult, IServiceScope scope);
}

public class CreateRootCaCommand : ICommand
{
    public IEnumerable<Option> Options => [_nameOption];
    public Command Command => new("create-root-ca", "루트 CA 생성");
    
    private readonly Option<string> _nameOption = new("--name", "name");
    
    public Task InvokeAsync(ParseResult o, IServiceScope scope)
    {
        Console.WriteLine($"루트 CA 생성: {o.GetValue(_nameOption)}");
        
        return Task.CompletedTask;
    }
}