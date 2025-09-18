using System.CommandLine;
using System.Reflection;
using CAService;
using Microsoft.Extensions.Hosting;
using Util;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.DiCaService();
    })
    .Build();

new RootCommand("유틸리티")
    .AddCommand(host, Assembly.GetExecutingAssembly())
    .Parse(args)
    .Invoke();
