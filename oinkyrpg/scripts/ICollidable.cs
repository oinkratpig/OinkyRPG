
namespace OinkyRPG;

public interface ICollidable
{
    /// <summary>
    /// Whether or not <see cref="RPGNodeMoveable"/>s can collide with this.
    /// </summary>
    public bool CollisionObstacle { get; }

} // end interface ICollidable