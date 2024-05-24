using Godot;

namespace OinkyRPG;

/// <summary>
/// An object taking space within a <see cref="RPGGrid"/>.
/// </summary>
[Tool]
public abstract partial class RPGNode : Node2D
{
    /// <summary>
    /// The grid being used.
    /// </summary>
    [Export]
    public RPGGrid Grid
    {
        get
        {
            if (_grid == null) _grid = LoadDefaultGrid();
            return _grid;
        }
        private set { _grid = value; }
    }

    private RPGGrid _grid;

    private static RPGGrid LoadDefaultGrid()
    {
        return ResourceLoader.Load<RPGGrid>("res://oinkyrpg/defaults/Grid.tres");

    } // end LoadDefaultGrid

} // end class RPGNode
