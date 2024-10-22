using Bogus;
using TaskTitan.Data;

public static class TaskItemGenerator
{
    public static Faker<TaskTitan.Data.TaskItem> Faker => new Faker<TaskTitan.Data.TaskItem>()
    .CustomInstantiator(f => new TaskTitan.Data.TaskItem(f.Rant.Random.Words()))
    .RuleFor(t => t.Uuid, (f, u) => Guid.NewGuid())
    .RuleFor(t => t.Entry, (f, t) => f.Date.Recent(60))
    .RuleFor(t => t.Modified, (f, t) => f.Date.Recent(20))
    .RuleFor<string>(t => t.Project, (f, t) => f.PickRandom<string>(new List<string?> { "Work", "Home", "SideHustle", null }))
    .RuleFor(t => t.Due, (f, t) => f.Date.Future().OrNull(f, .2f))
    .RuleFor(t => t.Status, (f, t) => f.PickRandom<TaskItemStatus>())
    .RuleFor(t => t.Urgency, (f, t) => f.Random.Double() * 10)
    ;
}
