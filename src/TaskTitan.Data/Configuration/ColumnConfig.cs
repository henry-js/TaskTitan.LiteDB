using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using static TaskTitan.Data.Enums;

namespace TaskTitan.Data;

public class AttributeColumnConfig
{
    [JsonIgnore]
    public string Name { get; set; }
    public bool IsModifiable { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ColFormat Format { get; private set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ColType ColType { get; set; }
    public IReadOnlyList<ColFormat>? AllowedFormats { get; }
    public HashSet<ColModifier> AllowedModifiers { get; }

    public AttributeColumnConfig(string name, bool isModifiable, ColFormat format, ColType type, List<ColFormat>? allowedFormats = null)
    {
        Name = name;
        IsModifiable = isModifiable;
        Format = format;
        ColType = type;
        AllowedFormats = allowedFormats ?? AttributeColumnFormats.AllowedFormats[type];
        AllowedModifiers = ColType switch
        {
            ColType.Date => [ColModifier.None, ColModifier.Not, ColModifier.Before, ColModifier.After, ColModifier.Is],
            ColType.Text => [ColModifier.None, ColModifier.Any, ColModifier.Is, ColModifier.Not, ColModifier.Has, ColModifier.Hasnt, ColModifier.Startswith, ColModifier.Endswith],
            ColType.Number => [ColModifier.None, ColModifier.Any, ColModifier.Below, ColModifier.Above, ColModifier.Is, ColModifier.Not],
            _ => throw new SwitchExpressionException(type)
        };
    }

    public void SetFormat(ColFormat format)
    {
        if (!AttributeColumnFormats.AllowedFormats.TryGetValue(ColType, out List<ColFormat>? allowed))
            throw new ArgumentException($"Unknown column type '{ColType}'");

        if (allowed.Contains(format))
        {
            Format = format;
        }
    }
}
