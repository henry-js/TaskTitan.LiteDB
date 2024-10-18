namespace TaskTitan.Data;

public static class Xdg
{
    private static readonly string? _dataHome;
    private static readonly string? _configHome;
    private static readonly string? _stateHome;

    static Xdg()
    {
        _dataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME", EnvironmentVariableTarget.User);
        _configHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME", EnvironmentVariableTarget.User);
        _stateHome = Environment.GetEnvironmentVariable("XDG_STATE_HOME", EnvironmentVariableTarget.User);
    }

    public static string? DATA_HOME => _dataHome;
    public static string? CONFIG_HOME => _configHome;
    public static string? STATE_HOME => _stateHome;

}
