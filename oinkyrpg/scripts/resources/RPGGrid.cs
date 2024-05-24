﻿using Godot;
using System.Collections.Generic;

namespace OinkyRPG;

/// <summary>
/// A grid used by all RPG nodes.
/// </summary>
[GlobalClass]
[Tool]
public partial class RPGGrid : Resource
{
    /// <summary>
    /// The width of each tile within the grid.
    /// </summary>
    [Export] public float TileWidth { get; private set; } = 32;

    /// <summary>
    /// The height of each tile within the grid.
    /// </summary>
    [Export] public float TileHeight { get; private set; } = 32;

    /// <summary>
    /// List of all active interactables within this grid.
    /// </summary>
    public List<RPGInteractable> Interactables { get; internal set; }

    /// <summary>
    /// Collection of all nodes that are marked for collision.
    /// </summary>
    public List<ICollidable> CollisionNodes { get; internal set; }

    public RPGGrid()
    {
        Interactables = new();
        CollisionNodes = new();

    } // end constructor

    /// <summary>
    /// Whether or not the tile with the given coordinates is marked as collision.
    /// </summary>
    public bool IsTileCollision(Vector2I gridPosition)
    {
        foreach(ICollidable collidable in CollisionNodes)
        {
            if(collidable is RPGNodeMoveable moveable &&
                gridPosition == moveable.DestinationGrid)
            {
                return true;
            }
            else if(collidable is RPGNodeStatic @static &&
                gridPosition == @static.GridPosition)
            {
                return true;
            }
        }
        return false;

    } // end IsTileCollision

    /// <summary>
    /// Converts a global position to grid coordinates.
    /// </summary>
    public Vector2I ToGridCoords(Vector2 globalPosition)
    {
        return new Vector2I(
            Mathf.RoundToInt(globalPosition.X / TileWidth),
            Mathf.RoundToInt(globalPosition.Y / TileHeight)
        );

    } // end ToGridCoords

    /// <summary>
    /// Converts grid coordinates to a global position.
    /// </summary>
    public Vector2 ToGlobalPosition(Vector2I gridCoordinates)
    {
        return new Vector2(
            gridCoordinates.X * TileWidth,
            gridCoordinates.Y * TileHeight
        );

    } // end ToGlobalPosition

} // end class RPGGrid