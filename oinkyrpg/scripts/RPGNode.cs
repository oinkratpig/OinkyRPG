using Godot;

namespace OinkyRPG;

/// <summary>
/// An object taking space within a <see cref="RPGGrid"/>.
/// </summary>
[GlobalClass]
public abstract partial class RPGNode : Node2D
{
    /// <summary>
    /// The grid being used.
    /// </summary>
    [Export] public RPGGrid Grid { get; private set; }

    public override void _EnterTree()
    {
        if(Grid == null)
        {
            Grid = ResourceLoader.Load<RPGGrid>("res://oinkyrpg/defaults/Grid.tres");
            if(RPGDebugSettings.WarnAboutGridDefault)
                GD.PushWarning("No Grid set, using default.");
        }

    } // end _EnterTree

} // end class RPGNode
