using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using TaskTitan.Cli.Commands;
using TaskTitan.Cli.Extensions;
using TaskTitan.Configuration;
using TaskTitan.Data;
using TaskTitan.Data.Reports;
using Tomlyn;

Global.CreateConfigurationDirectories();
var reports = Toml.ToModel<ReportDictionary>(File.ReadAllText(Path.Combine(Global.ConfigDirectoryPath, "reports.toml")));

var cmd = new RootCommand();
// cmd.AddGlobalOption(CliGlobalOptions.FilterOption);
cmd.AddCommand(new AddCommand());
cmd.AddCommand(new ListCommand(reports));


Global.LoadReports(reports);

var cmdLine = new CommandLineBuilder(cmd)
    .UseHost(_ => Host.CreateDefaultBuilder(args), builder =>
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json");
        })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(_ => AnsiConsole.Console);
                services.AddSingleton(f => new LiteDbContext(LiteDbContext.CreateConnectionStringFrom(Global.DataDirectoryPath)));
            })
            .UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration))
            .UseProjectCommandHandlers();
    })
    .UseDefaults()
    .Build();

int result = await cmdLine.InvokeAsync(args);

return result;
