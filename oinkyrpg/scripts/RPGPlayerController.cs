using Godot;
using GDCollection = Godot.Collections;
using System.Collections.Generic;
using System;

namespace OinkyRPG;
//
/// <summary>
/// The controllable player.
/// </summary>
[Tool]
public partial class RPGPlayerController : RPGNodeMoveable
{
    [Export] public RPGInput PlayerInput
    {
        get
        {
            if (_playerInput == null) _playerInput = ResourceLoader.Load<RPGInput>("res://oinkyrpg/defaults/Input.tres");
            return _playerInput;
        }
        private set { _playerInput = value; }
    }

    /// <summary>
    /// Whether or not diagonal movement is allowed.
    /// </summary>
    [Export] private bool _diagonalAllowed = true;

    private Dictionary<StringName, Vector2I> _inputVectors;
    private List<StringName> _movementInputQueue;
    private RPGInput _playerInput;

    public override void _Ready()
    {
        base._Ready();
        if (Engine.IsEditorHint()) return;

        _movementInputQueue = new();
        _inputVectors = new()
        {
            { PlayerInput.MoveWest, new Vector2I(-1, 0) },
            { PlayerInput.MoveEast, new Vector2I(1, 0) },
            { PlayerInput.MoveNorth, new Vector2I(0, -1) },
            { PlayerInput.MoveSouth, new Vector2I(0, 1) },
        };

    } // end _Ready

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Engine.IsEditorHint()) return;

        // Movement
        Vector2I move = Vector2I.Zero;
        foreach (StringName input in _movementInputQueue)
        {
            move += _inputVectors[input];
            if (!_diagonalAllowed) break;
        }
        MoveInDirection(move);

    } // end _Input

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (Engine.IsEditorHint()) return;
        
        // Movement
        foreach (StringName input in _inputVectors.Keys)
            if (@event.IsAction(input))
            {
                if (Input.IsActionJustPressed(input))
                    _movementInputQueue.Insert(0, input);
                if (!Input.IsActionPressed(input))
                    _movementInputQueue.Remove(input);
            }

        // Interact
        if (Input.IsActionJustPressed(PlayerInput.Interact))
            ActiveInteractable?.OnInteracted?.Invoke(this);

    } // end _Input

} // end class RPGPlayerController
