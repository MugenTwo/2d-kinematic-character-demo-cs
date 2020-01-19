using Godot;

public class Player : KinematicBody2D
{

    private static readonly float GRAVITY = 500;
    private static readonly float FLOOR_ANGLE_TOLERANCE = 40;
    private static readonly float WALK_FORCE = 600;
    private static readonly float WALK_MIN_SPEED = 10;
    private static readonly float WALK_MAX_SPEED = 200;
    private static readonly float STOP_FORCE = 1300;
    private static readonly float JUMP_SPEED = 200;
    private static readonly float JUMP_MAX_AIRBORNE_TIME = 0.2f;
    private static readonly float SLIDE_STOP_VELOCITY = 1.0f;
    private static readonly float SLIDE_STOP_MIN_TRAVEL = 1.0f;

    private Vector2 velocity = new Vector2(0, 0);
    private float onAirTime = 100;
    private bool jumping = false;
    private bool previousJumpPressed = false;

    public override void _PhysicsProcess(float delta)
    {
        Vector2 force = new Vector2(0, GRAVITY);
        bool isMoveLeftPressed = Input.IsActionPressed("move_left");
        bool isMoveRightPressed = Input.IsActionPressed("move_right");
        bool isJumpPressed = Input.IsActionPressed("jump");
        bool stop = true;

        if (isMoveLeftPressed)
        {
            if (velocity.x <= WALK_MIN_SPEED && velocity.x > -WALK_MAX_SPEED)
            {
                force.x -= WALK_FORCE;
                stop = false;
            }
        }
        else if (isMoveRightPressed)
        {
            if (velocity.x >= -WALK_MIN_SPEED && velocity.x < WALK_MAX_SPEED)
            {
                force.x += WALK_FORCE;
                stop = false;
            }
        }

        if (stop)
        {
            float velocitySign = Mathf.Sign(velocity.x);
            float velocityLength = Mathf.Abs(velocity.x);

            velocityLength -= STOP_FORCE * delta;
            if (velocityLength < 0)
            {
                velocityLength = 0;
            }
            velocity.x = velocityLength * velocitySign;
        }

        velocity += force * delta;
        velocity = MoveAndSlide(velocity, new Vector2(0, -1));

        if (IsOnFloor())
        {
            onAirTime = 0;
        }
        if (jumping && velocity.y > 0)
        {
            jumping = false;
        }
        if (onAirTime < JUMP_MAX_AIRBORNE_TIME && isJumpPressed && !previousJumpPressed && !jumping)
        {
            velocity.y = -JUMP_SPEED;
            jumping = true;
        }

        onAirTime += delta;
        previousJumpPressed = jumping;
    }


}
