using BulkyBook;
using Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).UseSerilog((context, config) => { config.WriteTo.Console(); }
        ).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>(); // Specify the Startup class
            });
}