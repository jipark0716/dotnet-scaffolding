using System.CommandLine;
using System.Security.Cryptography.X509Certificates;
using CAService;
using Microsoft.Extensions.DependencyInjection;

namespace Util;

public class CreateClientCaCommand : ICommand
{
    public IEnumerable<Option> Options => [_rootOption, _rootPasswordOption, _outputOption, _passwordOption];
    public Command Command => new("create-client-ca", "Client CA 생성");
    
    private readonly Option<string> _rootOption = new("--root", "root")
    {
        Required = true,
    };
    
    private readonly Option<string> _rootPasswordOption = new("--root-password", "root password")
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

        X509Certificate2 rootCert = service.ImportLocal(
            o.GetValue(_rootOption)!,
            o.GetValue(_rootPasswordOption)!);
        
        service.CreateRootAs(
            o.GetValue(_subjectOption)!,
            o.GetValue(_outputOption)!, 
            o.GetValue(_passwordOption)!);
        
        return Task.CompletedTask;
    }
}