using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Grpc.Net.Client;
using GrpcClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(GrpcChannel.ForAddress("https://localhost:1932", new GrpcChannelOptions
        {
            HttpHandler = new HttpClientHandler
            {
                ClientCertificates = { X509CertificateLoader.LoadPkcs12FromFile(
                    "/Users/parkjungin/workspace/dotnet/dotnet-scaffolding/angdou7013_dev.pfx", "") },
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            },
        }));
        services.AddGrpcClients(Assembly.GetExecutingAssembly());
        services.AddHostedService<HostedService>();
    })
    .Build()
    .Run();