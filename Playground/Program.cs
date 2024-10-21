﻿using Bogus;
using TaskTitan.Configuration;
using TaskTitan.Data;
using TaskTitan.Data.Expressions;
using TaskTitan.Data.Extensions;
using TaskTitan.Data.Parsers;
using TaskTitan.Data.Reports;
using Tomlyn;

var config = new DefaultConfiguration();
var reports = config.Reports;

File.WriteAllText(Path.Combine(Global.ConfigDirectoryPath, "reports.toml"), Toml.FromModel(reports));
reports = Toml.ToModel<ReportDictionary>(File.ReadAllText(Path.Combine(Global.ConfigDirectoryPath, "reports.toml")));
// Console.WriteLine($"{reports.Count} reports loaded");

var context = new LiteDbContext(LiteDbContext.CreateConnectionStringFrom(":memory:"));

var tasks = context.db.GetCollection<TaskItem>("tasks");

var sampleTasks = new Faker<TaskItem>()
    .CustomInstantiator(f => new TaskItem(f.Rant.Random.Words()))
    .RuleFor(t => t.Uuid, (f, u) => Guid.NewGuid())
    .RuleFor(t => t.Entry, (f, t) => f.Date.Recent(60))
    .RuleFor(t => t.Modified, (f, t) => f.Date.Recent(20))
    .RuleFor(t => t.Project, (f, t) => f.PickRandom(new List<string?> { "Work", "Home", "SideHustle", null }))
    .RuleFor(t => t.Due, (f, t) => f.Date.Future().OrNull(f, .2f))
    .RuleFor(t => t.Status, (f, t) => f.PickRandom<TaskItemStatus>())
    .RuleFor(t => t.Urgency, (f, t) => f.Random.Double() * 10)
    ;

var generatedTasks = sampleTasks.Generate(100);

tasks.InsertBulk(generatedTasks);

var dateParser = new DateParser(TimeProvider.System);
var attribute1 = TaskAttribute.Create("due.after", "tomorrow", dateParser);
var attribute2 = TaskAttribute.Create("project.contains", "work", dateParser);
var expr = new FilterExpression(new BinaryFilter(attribute1, BinaryOperator.Or, attribute2));

var filtered = tasks.Find(expr.ToBsonExpression());

Console.WriteLine(filtered.Count());
