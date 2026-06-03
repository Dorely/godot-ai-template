using Godot;

public partial class PlayerController : CharacterBody2D
{
    private static readonly StringName MoveLeftAction = new("move_left");
    private static readonly StringName MoveRightAction = new("move_right");
    private static readonly StringName JumpAction = new("jump");

    private bool _wasJumpPressed;

    [Export]
    public float MoveSpeed { get; set; } = 320.0f;

    [Export]
    public float JumpVelocity { get; set; } = -660.0f;

    [Export]
    public float Gravity { get; set; } = 1600.0f;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        float direction = (float)Input.GetAxis(MoveLeftAction, MoveRightAction);
        velocity.X = direction * MoveSpeed;

        if (!IsOnFloor())
        {
            velocity.Y += Gravity * (float)delta;
        }
        else if (velocity.Y > 0.0f)
        {
            velocity.Y = 0.0f;
        }

        bool isJumpPressed = Input.IsActionPressed(JumpAction);
        if (isJumpPressed && !_wasJumpPressed && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        _wasJumpPressed = isJumpPressed;

        Velocity = velocity;
        MoveAndSlide();
    }
}
