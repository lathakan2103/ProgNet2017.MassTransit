using Serilog;
using Topshelf;

namespace Demo.MassTransit
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogging();
            
            HostFactory.Run(x =>
            {
                x.UseSerilog();
                x.Service<Service>();
            });
        }
        
        private static void ConfigureLogging() =>
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .CreateLogger();
    }
}