﻿using Godot;
using System.Diagnostics;

namespace OinkyRPG;

/// <summary>
/// An object within a <see cref="RPGGrid"/>.<br/>
/// Changing position will result in a movement animation.
/// </summary>
[Tool]
public partial class RPGGridMoveable : RPGGridNode
{
    /// <summary>
    /// Determines how movement is handled.<br/>
    /// <see cref="Lerp"/>: Current position is interpolated to the resting position.<br/>
    /// <see cref="Speed"/>: Speed is added to the current position until resing position is met.<br/>
    /// <see cref="Teleport"/>: Teleports into place immediately.
    /// </summary>
    public enum MovementModes { Lerp, Speed, Teleport }

    [ExportGroup("Movement")]
    [Export] private MovementModes MovementMode { get; set; } = MovementModes.Lerp;
    [Export] private float LerpValue { get; set; } = 0.1f;
    [Export] private float SpeedValue { get; set; } = 1f;

    /// <summary>
    /// The desired destination that the node will move to.<br/>
    /// Uses global position rather than grid coordiantes.
    /// </summary>
    [ExportGroup("Position")]
    [Export]
    public Vector2 Destination
    {
        get { return _destination; }
        set
        {
            _destination = new Vector2(value.X.SnapTo(Grid.TileWidth), value.Y.SnapTo(Grid.TileHeight));
            if (Engine.IsEditorHint())
                GlobalPosition = _destination;
            _moving = true;
        }
    }

    /// <summary>
    /// The desired destination that the node will move to.<br/>
    /// Uses grid coordinates instead of global position.
    /// </summary>
    [Export]
    public Vector2I DestinationGrid
    {
        get { return Grid.ToGridCoords(Destination); }
        set { Destination = Grid.ToGlobalPosition(value); }
    }

    private Vector2 _destination;
    private bool _moving = false;

    public override void _Ready()
    {
        base._Ready();
        Destination = GlobalPosition;

    } // end _Ready

    public override void _PhysicsProcess(double delta)
    {
        // Movement
        if(_moving)
        {
            // Lerp movement
            if (MovementMode == MovementModes.Lerp)
                GlobalPosition = GlobalPosition.Lerp(Destination, LerpValue);

            // Speed movement
            else if (MovementMode == MovementModes.Speed)
                GlobalPosition = GlobalPosition.MoveToward(Destination, SpeedValue);

            // Stop
            if(GlobalPosition.DistanceTo(Destination) <= 0.5f)
            {
                _moving = false;
                GlobalPosition = Destination;
            }
        }
        
    } // end _PhysicsProcess

} // end class RPGGridMoveable
