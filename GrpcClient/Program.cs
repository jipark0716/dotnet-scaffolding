using System.Reflection;
using Grpc.Net.Client;
using GrpcClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(GrpcChannel.ForAddress("http://localhost:5000"));
        services.AddGrpcClients(Assembly.GetExecutingAssembly());
        services.AddHostedService<HostedService>();
    })
    .Build()
    .Run();