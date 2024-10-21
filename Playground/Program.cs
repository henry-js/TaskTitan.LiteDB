using TaskTitan.Configuration;
using TaskTitan.Data.Reports;
using Tomlyn;

var config = new DefaultConfiguration();
var reports = config.Reports;

File.WriteAllText(Path.Combine(Global.ConfigDirectoryPath, "reports.toml"), Toml.FromModel(reports));
reports = Toml.ToModel<ReportDictionary>(File.ReadAllText(Path.Combine(Global.ConfigDirectoryPath, "reports.toml")));
// Console.WriteLine($"{reports.Count} reports loaded");


Console.WriteLine($"Data path: {Global.DataDirectoryPath}");
Console.WriteLine($"Config path: {Global.ConfigDirectoryPath}");
Console.WriteLine($"State path: {Global.StateDirectoryPath}");
