using System.CommandLine;
using CAService;
using Microsoft.Extensions.DependencyInjection;

namespace Util;

public class CreateRootCaCommand : ICommand
{
    public IEnumerable<Option> Options => [_subjectOption, _outputOption, _passwordOption];
    public Command Command => new("create-root-ca", "루트 CA 생성");
    
    private readonly Option<string> _subjectOption = new("--subject", "subject")
    {
        Required = true,
    };
    
    private readonly Option<string> _outputOption = new("--output", "output")
    {
        Required = true,
    };
    
    private readonly Option<string> _passwordOption = new("--password", "password")
    {
        Required = true,
    };
    
    public Task InvokeAsync(ParseResult o, IServiceScope scope)
    {
        CaService service = scope.ServiceProvider.GetRequiredService<CaService>();
        
        service.CreateRootAs(
            o.GetValue(_subjectOption)!,
            o.GetValue(_outputOption)!, 
            o.GetValue(_passwordOption)!);
        
        return Task.CompletedTask;
    }
}