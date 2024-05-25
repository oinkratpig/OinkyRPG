using Godot;

namespace OinkyRPG;

/// <summary>
/// An object within a <see cref="RPGGrid"/>.<br/>
/// Changing position will immediately update its location.
/// </summary>
[Tool]
public partial class RPGNodeStatic : RPGNode, ICollidable
{
    /// <summary>
    /// Whether or not <see cref="RPGNodeMoveable"/>s can collide with this.
    /// </summary>
    [Export]
    public bool CollisionObstacle
    {
        get { return _collision; }
        private set
        {
            if (_collision == value) return;
            _collision = value;
            if (_collision)
                Grid.CollisionNodes.Add(this);
            else
                Grid.CollisionNodes.Remove(this);
        }
    }

    /// <summary>
    /// Current position in the grid.
    /// </summary>
    [Export]
    public Vector2I GridPosition
    {
        get { return Grid.ToGridCoords(GlobalPosition); }
        set {
            if (!IsInstanceValid(Grid)) return;
            GlobalPosition = Grid.ToGlobalPosition(value);
        }
    }

    private bool _collision;

    public override bool _Set(StringName property, Variant value)
    {
        // Force position to always snap to grid
        if (property == PropertyName.Position || property == PropertyName.GlobalPosition)
        {
            Vector2 newPosition = value.AsVector2();
            GlobalPosition = new Vector2(newPosition.X.SnapTo(Grid.TileWidth), newPosition.Y.SnapTo(Grid.TileHeight));
            return true;
        }

        return false;

    } // end _Set

} // end class RPGNodeStatic
