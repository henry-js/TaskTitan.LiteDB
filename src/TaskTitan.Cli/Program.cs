using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Configuration;
using TaskTitan.Cli.Extensions;
using TaskTitan.Cli.Commands;
using TaskTitan.Data;

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Debug()
            .WriteTo.File("logs/startup_.log",
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
            rollingInterval: RollingInterval.Day
            )
            .Enrich.WithProperty("Application Name", "<APP NAME>");
Log.Logger = loggerConfiguration.CreateBootstrapLogger();

var cmd = new CliRootCommand();

cmd.Add(new AddCommand().UseCommandHandler<AddCommand.Handler>());

var cmdLine = new CliConfiguration(cmd)
    .UseHost(_ => Host.CreateDefaultBuilder(args), builder =>
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json");
        })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(_ => AnsiConsole.Console);
                services.AddTransient(f => new LiteDbContext(context.Configuration.GetConnectionString("TempDb") ?? throw new NullReferenceException()));
            })
            .UseConsoleLifetime()
            .UseInvocationLifetime()
            .UseSerilog((context, services, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));
    })

    // .UseExceptionHandler((ex, context) =>
    // {
    //     AnsiConsole.WriteException(ex, ExceptionFormats.Default);
    //     Log.Fatal(ex, "Application terminated unexpectedly");
    // })
    .Build();

int result = await cmdLine.InvokeAsync(args);

return result;
