using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScrumPlanningBot.Core.Entities;
using ScrumPlanningBot.Core.Services;

namespace ScrumPlanningBot.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup Host
            var host = CreateDefaultBuilder().Build();

            // Invoke Worker
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var workerInstance = provider.GetRequiredService<Worker>();
            workerInstance.DoWork();

            host.Run();

            //CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateDefaultBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<Worker>();

                    // requires using Microsoft.Extensions.Options
                    services.Configure<MongoDbSettings>(
                        hostContext.Configuration.GetSection(nameof(MongoDbSettings)));

                    services.AddSingleton<IMongoDbSettings>(sp =>
                        sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

                    // add services
                    services.AddSingleton<BookService>();
                    services.AddSingleton<UserService>();
                    services.AddSingleton<RoomService>();
                    services.AddSingleton<IChatService, TelegramService>();

                    services.AddLogging();
                    services.AddBotCommands();
                    services.AddHostedService<Bot>();
                });
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .ConfigureAppConfiguration(x =>
                {
                    //x.AddUserSecrets<Program>();
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true)
                        .Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // requires using Microsoft.Extensions.Options
                    services.Configure<MongoDbSettings>(
                        hostContext.Configuration.GetSection(nameof(MongoDbSettings)));

                    services.AddSingleton<IMongoDbSettings>(sp =>
                        sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

                    // add services
                    services.AddSingleton<BookService>();
                    services.AddSingleton<UserService>();
                    services.AddSingleton<RoomService>();
                    services.AddSingleton<IChatService, TelegramService>();

                    services.AddLogging();
                    services.AddBotCommands();
                    services.AddHostedService<Bot>();
                });
    }

    // Worker.cs
    internal class Worker
    {
        private readonly IConfiguration configuration;

        public Worker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void DoWork()
        {
            var keyValuePairs = configuration.AsEnumerable().ToList();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==============================================");
            Console.WriteLine("Configurations...");
            Console.WriteLine("==============================================");
            foreach (var pair in keyValuePairs)
            {
                Console.WriteLine($"{pair.Key} - {pair.Value}");
            }
            Console.WriteLine("==============================================");
            Console.ResetColor();
        }
    }
}
