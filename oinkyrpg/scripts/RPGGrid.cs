using Godot;

namespace OinkyRPG;

/// <summary>
/// A grid used by all <see cref="RPGNode"/> children.
/// </summary>
[Tool]
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
