using Godot;
using System;
using System.Collections.Generic;

namespace OinkyRPG;

/// <summary>
/// <see cref="TileMap"/> for use with OinkyRPG.
/// </summary>
[Tool]
public partial class RPGTilemap : TileMap
{
    /// <summary>
    /// Layers that contain collision tiles.<br/>
    /// Any tile in these layer will be marked as a collision tile.
    /// </summary>
    [Export]
    public int[] CollisionLayers { get; private set; } = new int[0];

    /// <summary>
    /// The grid being used.
    /// </summary>
    [Export]
    public RPGGrid Grid
    {
        get
        {
            if (_grid == null) _grid = RPGGrid.LoadDefaultGrid();
            return _grid;
        }
        private set { _grid = value; }
    }

    private RPGGrid _grid;

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;

        foreach (int layer in CollisionLayers)
            foreach (Vector2I cell in GetUsedCells(layer))
            {
                Vector2I gridPosition = Grid.ToGridCoords(GlobalPosition) + cell;
                if(!Grid.CollisionPositions.Contains(gridPosition))
                    Grid.CollisionPositions.Add(gridPosition);
            }

    } // end _Ready

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

} // end class RPGCollisionTilemap