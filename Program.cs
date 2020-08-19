using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LoggingPoc
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class Program
    {
        private static void Main()
        {
            Console.WriteLine(".NET Core Console app logging example");

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogTrace("Trace in Program.cs");
            logger.LogDebug("Debug in Program.cs");
            logger.LogInformation("Log in Program.cs");
            logger.LogCritical("Critical in Program.cs");
            
            // Note since we aren't configuring the Logging with the AddDebug method, it causes the logging to not
            // flush the messages to the console if we are debugging or running from the command line. If we are using
            // Docker, the messages will displayed as expected.
            serviceProvider.Dispose();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(configure => configure
                .AddConsole()
                .AddConfiguration(configuration.GetSection("Logging"))
                .SetMinimumLevel(LogLevel.Information));
        }
    }
}