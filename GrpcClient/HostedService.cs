using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Hosting;
using V1.Help;

namespace GrpcClient;

public class HostedService(
    HelpService.HelpServiceClient helpService
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            HelpResponse helpResponse = await helpService.HealthAsync(new Empty());

            Console.WriteLine(helpResponse.Uptime.ToTimeSpan());
            Debug.Assert(!helpResponse.Health);
            await Task.Delay(1000, stoppingToken);
        }
    }
}