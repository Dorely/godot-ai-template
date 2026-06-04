using Godot;

public partial class PlayerController : CharacterBody2D
{
    private static readonly StringName MoveLeftAction = new("move_left");
    private static readonly StringName MoveRightAction = new("move_right");
    private static readonly StringName JumpAction = new("jump");

    private PlayerSpriteAnimator _spriteAnimator;
    private bool _wasAirborne;
    private bool _jumpConsumedForPress;

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
        if (!isJumpPressed)
        {
            _jumpConsumedForPress = false;
        }

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

        bool isGrounded = IsOnFloor();
        bool didLand = _wasAirborne && isGrounded;
        bool didJump = false;

        if (isJumpPressed && isGrounded && !_jumpConsumedForPress)
        {
            Vector2 jumpVelocity = Velocity;
            jumpVelocity.Y = JumpVelocity;
            Velocity = jumpVelocity;
            isGrounded = false;
            didJump = true;
            _jumpConsumedForPress = true;
        }

        UpdateSpriteAnimator(direction, isGrounded, didLand, didJump);
        _wasAirborne = !isGrounded;
    }

    private void UpdateSpriteAnimator(float direction, bool isGrounded, bool didLand, bool didJump)
    {
        if (_spriteAnimator == null)
        {
            return;
        }

        _spriteAnimator.SetFacing(direction);

        if (didJump || Velocity.Y < -0.01f)
        {
            _spriteAnimator.SetState(PlayerSpriteAnimator.MotionState.Jump);
        }
        else if (!isGrounded)
        {
            _spriteAnimator.SetState(PlayerSpriteAnimator.MotionState.Fall);
        }
        else if (didLand)
        {
            _spriteAnimator.SetState(PlayerSpriteAnimator.MotionState.Land);
        }
        else if (_spriteAnimator.IsLandingActive)
        {
            return;
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
