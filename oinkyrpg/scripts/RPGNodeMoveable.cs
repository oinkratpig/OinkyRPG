﻿using Godot;
using System;

namespace OinkyRPG;

/// <summary>
/// An object within a <see cref="RPGGrid"/>.<br/>
/// Changing position will result in a movement animation.
/// </summary>
[Tool]
public partial class RPGNodeMoveable : RPGNode, ICollidable
{
    /// <summary>
    /// Determines how movement is handled.<br/>
    /// <see cref="Lerp"/>: Current position is interpolated to the resting position.<br/>
    /// <see cref="Speed"/>: Speed is added to the current position until resing position is met.<br/>
    /// <see cref="Mixed"/>: Both lerp and speed.<br/>
    /// <see cref="Teleport"/>: Teleports into place immediately.
    /// </summary>
    public enum MovementModes { Speed, Lerp, Mixed, Teleport }

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
    /// Whether this moveable will detect interactable objects.
    /// </summary>
    [Export] public bool Interacting { get; private set; }

    /// <summary>
    /// The desired destination that the node will move to.<br/>
    /// Uses grid coordinates instead of global position.
    /// </summary>
    /// [ExportGroup("Position")]
    [Export]
    public Vector2I DestinationGrid
    {
        get { return Grid.ToGridCoords(Destination); }
        private set { Destination = Grid.ToGlobalPosition(value); }
    }

    [ExportGroup("Movement")]
    [Export] public MovementModes MovementMode { get; set; } = MovementModes.Speed;
    [Export] public float LerpValue { get; set; } = 0.1f;
    [Export] public float SpeedValue { get; set; } = 4f;

    /// <summary>
    /// The desired destination that the node will move to.<br/>
    /// Uses global position rather than grid coordiantes.
    /// </summary>
    public Vector2 Destination
    {
        get { return _destination; }
        private set
        {
            _destination = new Vector2(value.X.SnapTo(Grid.TileWidth), value.Y.SnapTo(Grid.TileHeight));
            if (Engine.IsEditorHint() || MovementMode == MovementModes.Teleport)
                GlobalPosition = _destination;

            Moving = true;
        }
    }

    public bool Moving { get; private set; } = false;

    /// <summary>
    /// Last angle of movement.<br/>
    /// (When destination is changed, the angle is calculated from current position to destination).
    /// </summary>
    public float LookAngle
    {
        get { return _lookAngle; }
        private set
        {
            if(_lookAngle != value)
            {
                _lookAngle = value;
                SetActiveInteractable();
            }
        }
    }

    /// <summary>
    /// Interactable being faced at the moment.
    /// </summary>
    public RPGInteractable ActiveInteractable { get; private set; }

    private bool _collision;
    private Vector2 _destination;
    private float _lookAngle;

    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint()) return;

        // Movement
        if(Moving)
        {
            // Lerp movement
            if (MovementMode == MovementModes.Lerp || MovementMode == MovementModes.Mixed)
                GlobalPosition = GlobalPosition.Lerp(Destination, LerpValue);

            // Speed movement
            if (MovementMode == MovementModes.Speed || MovementMode == MovementModes.Mixed)
                GlobalPosition = GlobalPosition.MoveToward(Destination, SpeedValue);

            // Stop moving
            if (GlobalPosition.DistanceTo(Destination) <= 0.5f)
            {
                Moving = false;
                GlobalPosition = Destination;
                if (Interacting)
                    SetActiveInteractable();
            }
        }
        
    } // end _PhysicsProcess

    public override bool _Set(StringName property, Variant value)
    {
        // Force position to always snap to grid
        if (Engine.IsEditorHint() &&
            (property == PropertyName.Position || property == PropertyName.GlobalPosition))
        {
            Vector2 newPosition = value.AsVector2();
            GlobalPosition = new Vector2(newPosition.X.SnapTo(Grid.TileWidth), newPosition.Y.SnapTo(Grid.TileHeight));
            Destination = GlobalPosition;
            return true;
        }

        return false;

    } // end _Set

    /// <summary>
    /// Returns <see cref="RPGInteractable"/> being faced.
    /// </summary>
    private void SetActiveInteractable()
    {
        Vector2I facing = new Vector2I(
                        Mathf.RoundToInt(DestinationGrid.X + Mathf.Cos(Mathf.DegToRad(LookAngle))),
                        Mathf.RoundToInt(DestinationGrid.Y + Mathf.Sin(Mathf.DegToRad(LookAngle))));

        RPGInteractable oldInteractable = ActiveInteractable;
        ActiveInteractable = null;
        foreach (RPGInteractable interactable in Grid.Interactables)
            if (interactable.GridPosition == facing)
            {
                ActiveInteractable = interactable;
                if (ActiveInteractable != interactable)
                    interactable.OnBeginActive?.Invoke(this);
                break;
            }

        if (IsInstanceValid(oldInteractable) && ActiveInteractable != oldInteractable)
            oldInteractable.OnEndActive?.Invoke(this);

    } // end SetActiveInteractable

    /// <summary>
    /// Move player's grid position in given combination of directions.
    /// </summary>
    /// <returns>If successful.</returns>
    public bool MoveInDirection(bool west, bool east, bool north, bool south)
    {
        // Can't move
        if (Moving || !(west || east || north || south)) return false;

        Vector2I destination = DestinationGrid;
        Vector2I hor = new Vector2I(-west.AsInt() + east.AsInt(), 0);
        Vector2I vert = new Vector2I(0, -north.AsInt() + south.AsInt());

        LookAngle = Mathf.RadToDeg(((Vector2)destination).AngleToPoint(destination + hor + vert)).FixAngleDegrees().SnapTo(45);

        // Horizontal collision
        if (!Grid.IsTileCollision(destination + hor))
            destination += hor;
        // Vertical collision
        if (!Grid.IsTileCollision(destination + vert))
            destination += vert;

        // Movement happened
        if (destination != DestinationGrid)
        {
            DestinationGrid = destination;
            ActiveInteractable = null;
        }

        return true;

    } // end MoveInDirection

    /// <summary>
    /// Move player's grid position by one tile in given direction.
    /// </summary>
    /// <returns>If successful.</returns>
    public bool MoveInDirection(Vector2I moveDirection)
    {
        return MoveInDirection(moveDirection.X < 0, moveDirection.X > 0, moveDirection.Y < 0, moveDirection.Y > 0);

    } // end MoveInDirection

} // end class RPGNodeMoveable
