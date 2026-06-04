using Godot;

public partial class PlayerSpriteAnimator : Sprite2D
{
    public enum MotionState
    {
        Idle,
        Run,
        Jump,
        Fall
    }

    private readonly SpriteFrame[] _idleFrames =
    {
        new(225, 28, 31, 35),
        new(260, 28, 31, 35),
        new(294, 28, 31, 35),
        new(329, 28, 31, 35)
    };

    private readonly SpriteFrame[] _runFrames =
    {
        new(4, 66, 31, 35),
        new(49, 66, 22, 35),
        new(74, 66, 25, 37),
        new(104, 67, 34, 36),
        new(144, 67, 35, 35),
        new(189, 67, 27, 35),
        new(221, 66, 25, 35),
        new(247, 66, 26, 37)
    };

    private readonly SpriteFrame[] _jumpFrames =
    {
        new(5, 147, 31, 38),
        new(36, 147, 17, 42),
        new(55, 145, 21, 47),
        new(79, 149, 24, 42)
    };

    private readonly SpriteFrame[] _fallFrames =
    {
        new(107, 149, 29, 43),
        new(138, 150, 25, 39),
        new(165, 152, 31, 33)
    };

    private MotionState _state = MotionState.Idle;
    private int _frameIndex;
    private float _frameTimer;

    public override void _Ready()
    {
        Centered = false;
        RegionEnabled = true;
        TextureFilter = CanvasItem.TextureFilterEnum.Nearest;
        ApplyFrame();
    }

    public override void _Process(double delta)
    {
        SpriteFrame[] frames = GetFrames(_state);
        if (frames.Length <= 1)
        {
            return;
        }

        _frameTimer += (float)delta;
        float frameDuration = GetFrameDuration(_state);

        while (_frameTimer >= frameDuration)
        {
            _frameTimer -= frameDuration;

            if (_state == MotionState.Jump)
            {
                _frameIndex = Mathf.Min(_frameIndex + 1, frames.Length - 1);
            }
            else
            {
                _frameIndex = (_frameIndex + 1) % frames.Length;
            }

            ApplyFrame();
        }
    }

    public void SetState(MotionState state)
    {
        if (_state == state)
        {
            return;
        }

        _state = state;
        _frameIndex = 0;
        _frameTimer = 0.0f;
        ApplyFrame();
    }

    public void SetFacing(float direction)
    {
        if (direction < -0.01f)
        {
            FlipH = true;
            ApplyFrame();
        }
        else if (direction > 0.01f)
        {
            FlipH = false;
            ApplyFrame();
        }
    }

    private void ApplyFrame()
    {
        SpriteFrame frame = GetFrames(_state)[_frameIndex];
        RegionRect = frame.Region;

        float pivotX = FlipH ? frame.Region.Size.X - frame.Pivot.X : frame.Pivot.X;
        Offset = new Vector2(-pivotX, -frame.Pivot.Y);
    }

    private SpriteFrame[] GetFrames(MotionState state)
    {
        return state switch
        {
            MotionState.Run => _runFrames,
            MotionState.Jump => _jumpFrames,
            MotionState.Fall => _fallFrames,
            _ => _idleFrames
        };
    }

    private static float GetFrameDuration(MotionState state)
    {
        return state switch
        {
            MotionState.Run => 0.08f,
            MotionState.Jump => 0.10f,
            MotionState.Fall => 0.12f,
            _ => 0.20f
        };
    }

    private readonly struct SpriteFrame
    {
        public SpriteFrame(float x, float y, float width, float height)
        {
            Region = new Rect2(x, y, width, height);
            Pivot = new Vector2(width * 0.5f, height);
        }

        public Rect2 Region { get; }

        public Vector2 Pivot { get; }
    }
}
