using Godot;

namespace OinkyRPG;

/// <summary>
/// An interactable object on the grid.
/// </summary>
[Tool]
public partial class RPGInteractable : RPGNodeStatic
{
    /// <summary>
    /// Called when interacted with.
    /// </summary>
    public InteractedHandler OnInteracted { get; set; }
    public delegate void InteractedHandler(RPGNodeMoveable sender);

    /// <summary>
    /// Called when in front of a moveable.
    /// </summary>
    public BeganActiveHandler OnBeginActive { get; set; }
    public delegate void BeganActiveHandler(RPGNodeMoveable sender);

    /// <summary>
    /// Called when no longer in front of a moveable.
    /// </summary>
    public EndedActiveHandler OnEndActive { get; set; }
    public delegate void EndedActiveHandler(RPGNodeMoveable sender);

    public override void _Ready()
    {
        base._Ready();
        if (Engine.IsEditorHint()) return;

        OnInteracted = (RPGNodeMoveable sender) => { GD.Print("Interacted!"); };
        //OnBeginActive = (RPGNodeMoveable sender) => { GD.Print("Begin Hover!"); };
        //OnEndActive = (RPGNodeMoveable sender) => { GD.Print("End hover!"); };

    } // end TrySetGridParent

    public override void _EnterTree()
    {
        base._EnterTree();
        if (Engine.IsEditorHint()) return;

        if(!Grid.Interactables.Contains(this))
            Grid.Interactables.Add(this);

    } // end _EnterTree

    public override void _ExitTree()
    {
        base._ExitTree();
        if (Engine.IsEditorHint()) return;

        Grid.Interactables.Remove(this);

    } // end _ExitTree

} // end class RPGInteractable