using Godot;

namespace OinkyRPG;

/// <summary>
/// An object taking space within a <see cref="RPGGrid"/>.
/// </summary>
[Tool]
public abstract partial class RPGGridNode : Node2D
{
    /// <summary>
    /// The grid being used.
    /// </summary>
    public RPGGrid Grid {
        get
        {
            if (!IsInstanceValid(_grid))
                _grid = TrySetGridParent();
            return _grid;
        }
        private set { _grid = value; }
    }

    private RPGGrid _grid;

    /// <summary>
    /// Attempt to set <see cref="Grid"/>.<br/>
    /// Prints an error if none are found.
    /// </summary>
    protected RPGGrid TrySetGridParent()
    {
        RPGGrid grid = this.GetParentOfType<RPGGrid>();
        if(grid == null)
            GD.PrintErr($"{Name}:{GetType()} No parent RPGGrid found.");
        return grid;

    } // end TrySetGridParent

} // end class RPGGridNode
