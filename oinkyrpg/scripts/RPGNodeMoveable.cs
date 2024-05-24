using Godot;
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
    public bool Collision
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
            LookAngle = Mathf.RadToDeg(GlobalPosition.AngleToPoint(_destination)).FixAngleDegrees().SnapTo(45);
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
    public float LookAngle { get; private set; }

    /// <summary>
    /// Interactable being faced at the moment.
    /// </summary>
    public RPGInteractable ActiveInteractable { get; private set; }

    private bool _collision;
    private Vector2 _destination;

    public override void _PhysicsProcess(double delta)
    {
        // Movement
        if(Moving)
        {
            // Lerp movement
            if (MovementMode == MovementModes.Lerp || MovementMode == MovementModes.Mixed)
                GlobalPosition = GlobalPosition.Lerp(Destination, LerpValue);

            // Speed movement
            if (MovementMode == MovementModes.Speed || MovementMode == MovementModes.Mixed)
                GlobalPosition = GlobalPosition.MoveToward(Destination, SpeedValue);

            // Stop
            if (GlobalPosition.DistanceTo(Destination) <= 0.5f)
            {
                Moving = false;
                GlobalPosition = Destination;

                // Find interactable in front of moveable
                if (Interacting)
                {
                    Vector2I facing = new Vector2I(
                        Mathf.RoundToInt(DestinationGrid.X + Mathf.Cos(Mathf.DegToRad(LookAngle))),
                        Mathf.RoundToInt(DestinationGrid.Y + Mathf.Sin(Mathf.DegToRad(LookAngle))));

                    RPGInteractable oldInteractable = ActiveInteractable;
                    ActiveInteractable = null;
                    foreach (RPGInteractable interactable in Grid.Interactables)
                        if (interactable.GridPosition == facing || interactable.GlobalPosition == GlobalPosition)
                        {
                            ActiveInteractable = interactable;
                            if(interactable != oldInteractable)
                                interactable.OnBeginActive?.Invoke(this);
                            break;
                        }
                    if (IsInstanceValid(oldInteractable) && ActiveInteractable != oldInteractable)
                        oldInteractable.OnEndActive?.Invoke(this);
                }
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
    /// Move player's grid position in given combination of directions.
    /// </summary>
    /// <returns>If successful.</returns>
    public bool MoveInDirection(bool west, bool east, bool north, bool south)
    {
        Vector2I destination = DestinationGrid + new Vector2I(-west.AsInt() + east.AsInt(), -north.AsInt() + south.AsInt());

        // If no movement made or would collide, don't move
        if (Moving || !(west || east || north || south) || Grid.IsTileCollision(destination))
            return false;

        DestinationGrid = destination;
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
