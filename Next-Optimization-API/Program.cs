using MeetingScheduler.API;
using Microsoft.AspNetCore;
using Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateWebHost(args);
        //RunSeeding(host);
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
         .UseSerilog((hostingContext, loggerConfiguration) =>
              loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    public static IWebHost CreateWebHost(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>()
              .Build();

    //private static void RunSeeding(IWebHost host)
    //{
    //    var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
    //    using (var scope = scopeFactory.CreateScope())
    //    {
    //        var seeder = scope.ServiceProvider.GetService<ISeeder>();
    //        seeder.Seed().Wait();
    //    }
    //}
}