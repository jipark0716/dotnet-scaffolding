using System.Reflection;
using Lib.DI;
using Microsoft.Extensions.DependencyInjection;

namespace HelpService;

public static class DiExtension
{
    public static void DiHelpService(this IServiceCollection source) => source.Di(Assembly.GetExecutingAssembly());
}