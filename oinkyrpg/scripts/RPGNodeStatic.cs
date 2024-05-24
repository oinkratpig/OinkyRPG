using Godot;

namespace OinkyRPG;

/// <summary>
/// An object within a <see cref="RPGGrid"/>.<br/>
/// Changing position will immediately update its location.
/// </summary>
public partial class RPGNodeStatic : RPGNode
{
    [ExportGroup("Position")]
    [Export]
    public Vector2I GridPosition
    {
        get { return Grid.ToGridCoords(GlobalPosition); }
        set {
            if (!IsInstanceValid(Grid)) return;
            GlobalPosition = Grid.ToGlobalPosition(value);
        }
    }

    public override bool _Set(StringName property, Variant value)
    {
        // Force position to always snap to grid
        if (property == "position" || property == "global_position")
        {
            Vector2 newPosition = value.AsVector2();
            GlobalPosition = new Vector2(newPosition.X.SnapTo(Grid.TileWidth), newPosition.Y.SnapTo(Grid.TileHeight));
            return true;
        }

        return false;

    } // end _Set

} // end class RPGNodeStatic
