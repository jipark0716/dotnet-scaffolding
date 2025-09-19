using System.CommandLine;
using System.Security.Cryptography.X509Certificates;
using CAService;
using Microsoft.Extensions.DependencyInjection;

namespace Util;

public class CreateClientCaCommand : ICommand
{
    public IEnumerable<Option> Options => [_rootOption, _rootPasswordOption, _subjectOption, _outputOption];
    public Command Command => new("create-client-ca", "Client CA 생성");
    
    private readonly Option<string> _rootOption = new("--root", "root")
    {
        Required = true,
    };
    
    private readonly Option<string> _rootPasswordOption = new("--root-password", "root-password")
    {
        Required = true,
    };
    
    private readonly Option<string> _subjectOption = new("--subject", "subject")
    {
        Required = true,
    };
    
    private readonly Option<string> _outputOption = new("--output", "output")
    {
        Required = true,
    };
    
    public Task InvokeAsync(ParseResult o, IServiceScope scope)
    {
        CaService service = scope.ServiceProvider.GetRequiredService<CaService>();

        X509Certificate2 rootCert = service.ImportLocal(
            o.GetValue(_rootOption)!,
            o.GetValue(_rootPasswordOption)!);
        
        service.CreateClientAs(
            rootCert,
            o.GetValue(_subjectOption)!,
            o.GetValue(_outputOption)!,
            [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20],
            TimeSpan.FromDays(30));
        
        return Task.CompletedTask;
    }
}