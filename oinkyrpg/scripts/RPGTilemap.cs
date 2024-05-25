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

    /// <summary>
    /// Layers that contain collision tiles.<br/>
    /// Any tile in these layer will be marked as a collision tile.
    /// </summary>
    [Export]
    public int[] CollisionLayers { get; private set; } = new int[0];

    /// <summary>
    /// Offset of the tilemap collisions relative to the grid.<br/>
    /// The top-left of the tilemap will be placed at posision <see cref="CollisionOffset"/> in the <see cref="RPGGrid"/>.
    /// </summary>
    [Export]
    public Vector2I CollisionOffset { get; private set; } = Vector2I.Zero;

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;

        foreach (int layer in CollisionLayers)
            foreach (Vector2I cell in GetUsedCells(layer))
            {
                Vector2I gridPosition = cell + CollisionOffset;
                if(!Grid.CollisionPositions.Contains(gridPosition))
                    Grid.CollisionPositions.Add(gridPosition);
            }

    } // end _Ready

} // end class RPGCollisionTilemap