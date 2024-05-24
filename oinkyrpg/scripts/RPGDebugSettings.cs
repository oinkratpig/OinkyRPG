
namespace OinkyRPG;

/// <summary>
/// Settings for debugging OinkyRPG stuffs.
/// </summary>
internal static class RPGDebugSettings
{
    /// <summary>
    /// Whether to warn about the default grid being used.<br/>
    /// If the Grid property is not set in the inspector, <see cref="RPGNode"/>s will use the default.<br/>
    /// Default located in res://oinkyrpg/defaults/Grid.tres.
    /// </summary>
    public static bool WarnAboutGridDefault { get; internal set; } = false;

} // end class RPGDebugSettings