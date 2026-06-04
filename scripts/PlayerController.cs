using Godot;

public partial class PlayerController : CharacterBody2D
{
    private static readonly StringName MoveLeftAction = new("move_left");
    private static readonly StringName MoveRightAction = new("move_right");
    private static readonly StringName JumpAction = new("jump");

    private PlayerSpriteAnimator _spriteAnimator;

    [Export]
    public float MoveSpeed { get; set; } = 320.0f;

    [Export]
    public float JumpVelocity { get; set; } = -660.0f;

    [Export]
    public float Gravity { get; set; } = 1600.0f;

    [Export]
    public NodePath SpriteAnimatorPath { get; set; } = new("PlayerSprite");

    public override void _Ready()
    {
        if (SpriteAnimatorPath != null && SpriteAnimatorPath.ToString().Length > 0)
        {
            _spriteAnimator = GetNodeOrNull<PlayerSpriteAnimator>(SpriteAnimatorPath);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        float direction = (float)Input.GetAxis(MoveLeftAction, MoveRightAction);
        velocity.X = direction * MoveSpeed;
        bool isJumpPressed = IsJumpPressed();

        if (!IsOnFloor())
        {
            velocity.Y += Gravity * (float)delta;
        }
        else if (velocity.Y > 0.0f)
        {
            velocity.Y = 0.0f;
        }

        Velocity = velocity;
        MoveAndSlide();

        if (isJumpPressed && IsOnFloor())
        {
            Vector2 jumpVelocity = Velocity;
            jumpVelocity.Y = JumpVelocity;
            Velocity = jumpVelocity;
        }

        UpdateSpriteAnimator(direction);
    }

    private void UpdateSpriteAnimator(float direction)
    {
        if (_spriteAnimator == null)
        {
            return;
        }

        _spriteAnimator.SetFacing(direction);

        if (Velocity.Y < -0.01f)
        {
            _spriteAnimator.SetState(PlayerSpriteAnimator.MotionState.Jump);
        }
        else if (!IsOnFloor())
        {
            _spriteAnimator.SetState(PlayerSpriteAnimator.MotionState.Fall);
        }
        else if (Mathf.Abs(direction) > 0.01f)
        {
            _spriteAnimator.SetState(PlayerSpriteAnimator.MotionState.Run);
        }
        else
        {
            _spriteAnimator.SetState(PlayerSpriteAnimator.MotionState.Idle);
        }
    }

    private static bool IsJumpPressed()
    {
        return Input.IsActionPressed(JumpAction)
            || Input.IsKeyPressed(Key.Space)
            || Input.IsKeyPressed(Key.W)
            || Input.IsKeyPressed(Key.Up);
    }
}
