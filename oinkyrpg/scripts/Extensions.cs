using Godot;

namespace OinkyRPG;

public static class Extensions
{
    /// <summary>
    /// Snaps a number to the given value (rounding).<br/>
    /// For example, number "17" snapped to "32" would be "32".<br/>
    /// "16" snapped to "32" would be "32".<br/>
    /// "15" snapped to "32" would be "0".<br/>
    /// "35" snapped to "32" would be "32".
    /// </summary>
    /// <param name="number">The number to snap.</param>
    /// <param name="value">The value to snap to.</param>
    /// <returns>The snapped value.</returns>
    public static float SnapTo(this float number, float value)
    {
        return Mathf.Round(number / value) * value;

    } // end SnapTo

    /// <summary>
    /// Clamps an angle in degrees between 0 and 360.
    /// </summary>
    public static float FixAngleDegrees(this float angle)
    {
        while (angle > 360) angle -= 360;
        while (angle < 0) angle += 360;
        return angle;

    } // end FixAngle

    /// <summary>
    /// Get a <see cref="Node"/>'s closest parent of type.
    /// </summary>
    /// <returns>Parent of type or default.</returns>
    public static T GetParentOfType<T>(this Node node)
    {
        Node parent = node.GetParent();
        if(parent != null)
        {
            if (parent is T ret)
                return ret;
            else
                return parent.GetParentOfType<T>();
        }
        return default;

    } // end GetParentOfType

} // end class Extensions