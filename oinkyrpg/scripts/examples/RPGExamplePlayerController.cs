using Godot;

namespace OinkyRPG;

public partial class RPGExamplePlayerController : Node
{
    [Export] private RPGNodeMoveable _player;

    public override void _PhysicsProcess(double delta)
    {
        Vector2I move = Vector2I.Zero;
        if(!HandleMove(ref move, "rpg_example_move_north", 0, -1))
            HandleMove(ref move, "rpg_example_move_south", 0, 1);
        if (!HandleMove(ref move, "rpg_example_move_west", -1, 0))
            HandleMove(ref move, "rpg_example_move_east", 1, 0);
        _player.Move(move.X < 0, move.X > 0, move.Y < 0, move.Y > 0);

    } // end _Input

    private bool HandleMove(ref Vector2I move, string input, int hor, int vert)
    {
        if (Input.IsActionJustPressed(input)
            || Input.IsActionPressed(input) && !_player.Moving && _player.MovementMode != RPGNodeMoveable.MovementModes.Teleport)
        {
            move.X += hor;
            move.Y += vert;
            return true;
        }
        return false;

    } // end HandleMove

} // end class RPGNodeStatic
