namespace Platformer.AI
{
    public enum GroundMovementState
    {
        Stop,
        MoveForward,
        Jump,
        Patrol,
        Dash,
        Chase,
        Attack
    }

    public enum AerialMovementState
    {
        Stop,
        Dash,
        Float,
        MoveTowards,
        Move,
        Patrol,
        Animated
    }

    public enum CollisionBehaviour
    {
        None,
        Rebound,
        Fall,
        Explode,
        Disappear
    }
}
