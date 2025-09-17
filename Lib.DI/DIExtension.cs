using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Lib.DI;

public static class DiExtension
{
    public static void Di(this IServiceCollection source, Assembly asm)
    {
        IEnumerable<Type> types = asm.GetTypes()
            .Where(t =>
                typeof(IDependencyInjectable).IsAssignableFrom(t) &&
                t is { IsClass: true, IsAbstract: false }
            );

        foreach (Type t in types)
        {
            if (typeof(IScoped).IsAssignableFrom(t))
            {
                Type? scopedGeneric = t.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IScoped<>));

                if (scopedGeneric is null)
                {
                    source.AddScoped(t);
                    continue;
                }
                
                source.AddScoped(scopedGeneric.GetGenericArguments()[0], t);
            }
            else if (typeof(ISingleton).IsAssignableFrom(t))
            {
                Type? scopedGeneric = t.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISingleton<>));

                if (scopedGeneric is null)
                {
                    source.AddSingleton(t);
                    continue;
                }
                
                source.AddSingleton(scopedGeneric.GetGenericArguments()[0], t);
            }
        }
    }
}