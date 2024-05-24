/*
using Godot;
using System.Collections.Generic;

namespace OinkyRPGOLD;

/// <summary>
/// A grid used by all <see cref="RPGNode"/> children.
/// </summary>
public partial class RPGGrid : Node
{
    /// <summary>
    /// The width of each tile within the grid.
    /// </summary>
    [Export] public float TileWidth { get; private set; } = 1;

    /// <summary>
    /// The height of each tile within the grid.
    /// </summary>
    [Export] public float TileHeight { get; private set; } = 1;

    /// <summary>
    /// List of active <see cref="RPGInteractable"/>s in the scene.
    /// </summary>
    internal List<RPGInteractable> Interactables { get; private set; }

    public override void _EnterTree()
    {
        Interactables = new List<RPGInteractable>();

    } // end _EnterTree

    /// <summary>
    /// Registers an interactable for use within the grid.
    /// </summary>
    internal void AddInteractable(RPGInteractable interactable)
    {
        if(!Interactables.Contains(interactable))
            Interactables.Add(interactable);

    } // end AddInteractable

    /// <summary>
    /// Removes an interactable from use within the grid.
    /// </summary>
    internal void RemoveInteractable(RPGInteractable interactable)
    {
        if (Interactables.Contains(interactable))
            Interactables.Remove(interactable);

    } // end AddInteractable

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
*/