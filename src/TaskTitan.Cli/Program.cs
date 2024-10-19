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

Global.CreateConfigurationDirectories();

var cmd = new CliRootCommand();
cmd.Add(CliGlobalOptions.FilterOption);
cmd.UseCommandHandler<ListCommand.Handler>();

cmd.Add(new AddCommand());

var cmdLine = new CliConfiguration(cmd)
    .UseHost(_ => Host.CreateDefaultBuilder(args), builder =>
    {
        builder.ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json", false))
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(_ => AnsiConsole.Console);
                services.AddTransient(f => new LiteDbContext(LiteDbContext.CreateConnectionStringFrom(Global.DataDirectoryPath)));
            })
            .UseConsoleLifetime()
            .UseInvocationLifetime();
    });

int result = await cmdLine.InvokeAsync(args);

return result;
