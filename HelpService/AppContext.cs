using Lib.DI;

namespace HelpService;

public class AppContext : ISingleton
{
    private readonly DateTime _startDateTime = DateTime.Now;
    
    public TimeSpan Uptime() => DateTime.Now - _startDateTime;
}