using System.Reflection;
using GrpcServer;
using GrpcServer.Services.V1;
using HelpService;
using Lib.DI;
using Lib.MTlsManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMTls();
builder.Services.Di(Assembly.GetExecutingAssembly());
    
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(1932, static o =>
    {
        o.Protocols = HttpProtocols.Http2;
        o.UseMTls("/Users/parkjungin/workspace/dotnet/dotnet-scaffolding/root_dev.pfx", "lunaremote");
    });
    options.AddServerHeader = false;
});

builder.Services.DiHelpService();

WebApplication app = builder.Build();

app.MapGrpcService<HelpServiceServer>();
app.MapGrpcReflectionService();

app.Run();