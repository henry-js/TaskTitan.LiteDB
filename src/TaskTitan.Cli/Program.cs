using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Hosting;
using Microsoft.Extensions.Configuration;
using TaskTitan.Cli.Commands;
using TaskTitan.Data;
using TaskTitan.Cli.Logging;
using TaskTitan.Configuration;
using Microsoft.Extensions.Logging;
using TaskTitan.Cli.Extensions;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

Global.CreateConfigurationDirectories();

// var cmd = new CliRootCommand();
// cmd.Add(CliGlobalOptions.FilterOption);
// cmd.Add(new AddCommand().UseCommandHandler<AddCommand.Handler>());
// cmd.Add(new ListCommand().UseCommandHandler<ListCommand.Handler>());

// var path = $@"Filename={Global.DataDirectoryPath}\tasktitan.db";

// var cmdLine = new CliConfiguration(cmd)
//     .UseHost(_ => Host.CreateDefaultBuilder(args), builder =>
//     {
//         builder.ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json", false))
//             .ConfigureLogging((c, l) => l.ClearProviders())
//             .ConfigureServices((context, services) =>
//             {
//                 services.AddTransient(_ => AnsiConsole.Console);
//                 services.AddTransient(f => new LiteDbContext(path));
//             });
//     });

// int result = await cmdLine.InvokeAsync(args);

// return result;


var cmd = new RootCommand();
cmd.AddGlobalOption(CliGlobalOptions.FilterOption);
cmd.AddCommand(new AddCommand());
cmd.AddCommand(new ListCommand());

var path = $@"Filename={Global.DataDirectoryPath}\tasktitan.db";

var cmdLine = new CommandLineBuilder(cmd)
    .UseHost(_ => Host.CreateDefaultBuilder(args), builder =>
    {
        builder.ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json", false))
            .ConfigureLogging((c, l) => l.ClearProviders())
            .ConfigureServices((context, services) =>
            {
                services.AddTransient(_ => AnsiConsole.Console);
                services.AddTransient(f => new LiteDbContext(path));
            })
            .UseProjectCommandHandlers();
    }).Build();

int result = await cmdLine.InvokeAsync(args);

return result;
