using System.Reflection;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcClient;

public static class DiExtension
{
    public static void AddGrpcClients(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<Type> types = assembly.GetTypes()
            .Where(t =>
                t is { IsAbstract: false, BaseType.IsGenericType: true } &&
                t.BaseType.GetGenericTypeDefinition() == typeof(ClientBase<>));

        foreach (Type t in types)
        {
            services.AddSingleton(t, o => Activator.CreateInstance(t, o.GetRequiredService<GrpcChannel>())!);
        }
    }
}