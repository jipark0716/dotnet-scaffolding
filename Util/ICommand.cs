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