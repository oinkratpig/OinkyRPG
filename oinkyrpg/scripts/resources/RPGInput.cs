using Godot;
using System.Collections.Generic;
using GDCollection = Godot.Collections;

namespace OinkyRPG;
//
/// <summary>
/// Holds Godot input action strings for corresponding RPG player input.
/// </summary>
[GlobalClass]
[Tool]
public partial class RPGInput : Resource
{
    /// <summary>
    /// Press to interact with an <see cref="RPGInteractable"/> if facing it.
    /// </summary>
    [Export] public StringName Interact { get; private set; } = "oinkyrpg_interact";

    /// <summary>
    /// Press to move player north.
    /// </summary>
    [ExportGroup("Movement")]
    [Export] public StringName MoveNorth { get; private set; } = "oinkyrpg_move_north";

    /// <summary>
    /// Press to move player south.
    /// </summary>
    [Export] public StringName MoveSouth { get; private set; } = "oinkyrpg_move_south";

    /// <summary>
    /// Press to move player west.
    /// </summary>
    [Export] public StringName MoveWest { get; private set; } = "oinkyrpg_move_west";

    /// <summary>
    /// Press to move player east.
    /// </summary>
    [Export] public StringName MoveEast { get; private set; } = "oinkyrpg_move_east";

} // end class RPGInput