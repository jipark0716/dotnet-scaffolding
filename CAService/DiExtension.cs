using System.Reflection;
using Lib.DI;
using Microsoft.Extensions.DependencyInjection;

namespace CAService;

public static class DiExtension
{
    public static void DiCaService(this IServiceCollection source) => source.Di(Assembly.GetExecutingAssembly());
}