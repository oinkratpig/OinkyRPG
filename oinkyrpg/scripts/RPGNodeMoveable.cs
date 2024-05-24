using Godot;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace OinkyRPG;

/// <summary>
/// An object within a <see cref="RPGGrid"/>.<br/>
/// Changing position will result in a movement animation.
/// </summary>
public partial class RPGNodeMoveable : RPGNode
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
    /// Whether this moveable will detect interactable objects.
    /// </summary>
    [Export] public bool InteractingEnabled { get; private set; }

    [ExportGroup("Movement")]
    [Export] public MovementModes MovementMode { get; set; } = MovementModes.Speed;
    [Export] public float LerpValue { get; set; } = 0.1f;
    [Export] public float SpeedValue { get; set; } = 4f;

    /// <summary>
    /// The desired destination that the node will move to.<br/>
    /// Uses global position rather than grid coordiantes.
    /// </summary>
    [ExportGroup("Position")]
    [Export]
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

    /// <summary>
    /// The desired destination that the node will move to.<br/>
    /// Uses grid coordinates instead of global position.
    /// </summary>
    [Export]
    public Vector2I DestinationGrid
    {
        get { return Grid.ToGridCoords(Destination); }
        private set { Destination = Grid.ToGlobalPosition(value); }
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
                if (InteractingEnabled)
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

    /// <summary>
    /// Move player's grid position by the given amount.
    /// </summary>
    public void Move(Vector2I amount)
    {
        if(amount != Vector2I.Zero)
            DestinationGrid += amount;

    } // end Move

} // end class RPGNodeMoveable
