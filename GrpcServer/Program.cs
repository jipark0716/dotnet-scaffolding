using GrpcServer.Services.V1;
using HelpService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
    options.AddServerHeader = false;
});

builder.Services.DiHelpService();

WebApplication app = builder.Build();

app.MapGrpcService<HelpServiceServer>();
app.MapGrpcReflectionService();

app.Run();