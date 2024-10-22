
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using Playground;
using Bogus;
using TaskTitan.Data;

public class LiteDbBenchmark
{
    private LiteDbTaskStore _taskStore;

    [Params(50, 500, 5000)]
    public int TaskCount;

    [GlobalSetup]
    public void Setup()
    {
        _taskStore = new LiteDbTaskStore("TaskTitan.db");
    }

    private List<TaskItem> GenerateTasks(int count)
    {
        Faker<TaskItem> Faker = new Faker<TaskItem>()
        .CustomInstantiator(f => new TaskItem(f.Rant.Random.Words()))
        .RuleFor(t => t.Uuid, (f, u) => Guid.NewGuid())
        .RuleFor(t => t.Entry, (f, t) => f.Date.Recent(60))
        .RuleFor(t => t.Modified, (f, t) => f.Date.Recent(20))
        .RuleFor(t => t.Project, (f, t) => f.PickRandom(new List<string?> { "Work", "Home", "SideHustle", null }))
        .RuleFor(t => t.Due, (f, t) => f.Date.Future().OrNull(f, .2f))
        .RuleFor(t => t.Status, (f, t) => f.PickRandom<TaskItemStatus>())
        .RuleFor(t => t.Urgency, (f, t) => f.Random.Double() * 10)
        ;

        return Faker.Generate(count);
    }

    [Benchmark]
    public void InsertTasks()
    {
        var tasks = GenerateTasks(TaskCount);
        foreach (var task in tasks)
        {
            _taskStore.InsertTask(task);
        }
    }

    [Benchmark]
    public void RetrieveTasks()
    {
        for (int i = 1; i <= TaskCount; i++)
        {
            _taskStore.GetTask(i);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<LiteDbBenchmark>();
    }
}
