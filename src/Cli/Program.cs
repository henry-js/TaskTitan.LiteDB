﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Configuration;
using Data;
using Cli.Commands;
using Cli.Extensions;

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Debug()
            // .WriteTo.File("logs/startup_.log",
            // outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
            // rollingInterval: RollingInterval.Day
            // )
            // .Enrich.WithProperty("Application Name", "<APP NAME>");
            .WriteTo.Console();
Log.Logger = loggerConfiguration.CreateBootstrapLogger();


var rootCommand = new RootCommand("root");
rootCommand.AddCommand(new AddCommand());


var cmdLine = new CommandLineBuilder(rootCommand)
    .UseHost(_ => Host.CreateDefaultBuilder(args), builder =>
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json");
        })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(_ => AnsiConsole.Console);
                services.AddTransient(f => new LiteDbContext(context.Configuration.GetConnectionString("TempDb")));
            })
            .UseProjectCommandHandlers()
            .UseSerilog((context, services, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));
    })
    .UseDefaults()
    // .UseExceptionHandler((ex, context) =>
    // {
    //     AnsiConsole.WriteException(ex, ExceptionFormats.Default);
    //     Log.Fatal(ex, "Application terminated unexpectedly");
    // })
    .Build();

int result = await cmdLine.InvokeAsync(args);

return result;
