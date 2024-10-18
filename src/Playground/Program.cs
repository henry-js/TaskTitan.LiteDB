using System.Text.Json;
using TaskTitan.Data;
using TaskTitan.Data.Reports;
using Tomlyn;

// var config = new DefaultConfiguration();
// var reports = config.Reports;

// File.WriteAllText(@"C:\temp\tasktitan-reports.json", JsonSerializer.Serialize(reports, new JsonSerializerOptions() { WriteIndented = true }));

// File.WriteAllText(@"C:\temp\tasktitan-reports.toml", Toml.FromModel(reports));
var reports = Toml.ToModel<ReportDictionary>(File.ReadAllText(@"C:\temp\tasktitan-reports.toml"));

Console.WriteLine($"{reports.Count} reports loaded");
