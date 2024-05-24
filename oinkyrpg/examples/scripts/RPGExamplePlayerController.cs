using Godot;
using System;
using System.Collections.Generic;

namespace OinkyRPG;

public partial class RPGExamplePlayerController : Node
{
    [Export] private RPGNodeMoveable _player;
    [Export] private bool _diagonalAllowed;

    private List<StringName> _movementInputQueue;
    private Dictionary<StringName, Vector2I> _inputVectors;

    public override void _Ready()
    {
        _movementInputQueue = new();
        _inputVectors = new()
        {
            { "rpg_example_move_west", new Vector2I(-1, 0) },
            { "rpg_example_move_east", new Vector2I(1, 0) },
            { "rpg_example_move_north", new Vector2I(0, -1) },
            { "rpg_example_move_south", new Vector2I(0, 1) },
        };

    } // end _Ready

    public override void _PhysicsProcess(double delta)
    {
        Vector2I move = Vector2I.Zero;
        foreach (StringName input in _movementInputQueue)
            move += _inputVectors[input];
        _player.MoveInDirection(move);

    } // end _Input

    public override void _Input(InputEvent @event)
    {
        foreach (StringName input in _inputVectors.Keys)
            if (@event.IsAction(input))
            {
                if (Input.IsActionJustPressed(input))
                    _movementInputQueue.Insert(0, input);
                if (!Input.IsActionPressed(input))
                    _movementInputQueue.Remove(input);
            }

    } // end _Input

} // end class RPGNodeStatic
