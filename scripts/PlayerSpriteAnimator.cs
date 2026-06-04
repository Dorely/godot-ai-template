using Godot;

public partial class PlayerSpriteAnimator : Sprite2D
{
    public enum MotionState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Land
    }

    private enum ClipId
    {
        Idle,
        RunStart,
        RunLoop,
        Jump,
        Fall,
        Land
    }

    private readonly SpriteFrame[] _idleFrames =
    {
        new(225, 28, 31, 35),
        new(260, 28, 31, 35),
        new(294, 28, 31, 35),
        new(329, 28, 31, 35)
    };

    private readonly SpriteFrame[] _runStartFrames =
    {
        new(4, 66, 31, 35)
    };

    private readonly SpriteFrame[] _runLoopFrames =
    {
        new(49, 66, 22, 35),
        new(74, 66, 25, 37),
        new(104, 67, 34, 36),
        new(144, 67, 35, 35),
        new(189, 67, 27, 35),
        new(221, 66, 25, 35),
        new(247, 66, 26, 37),
        new(280, 67, 30, 34),
        new(318, 68, 34, 33),
        new(359, 68, 29, 33)
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
        new(107, 149, 29, 43)
    };

    private readonly SpriteFrame[] _landFrames =
    {
        new(138, 150, 25, 39),
        new(165, 152, 31, 33)
    };

    private MotionState _state = MotionState.Idle;
    private ClipId _clipId = ClipId.Idle;
    private int _frameIndex;
    private float _frameTimer;
    private bool _clipFinished;

    public bool IsLandingActive => _state == MotionState.Land && !_clipFinished;

    public override void _Ready()
    {
        Centered = false;
        RegionEnabled = true;
        TextureFilter = CanvasItem.TextureFilterEnum.Nearest;
        ApplyFrame();
    }

    public override void _Process(double delta)
    {
        Clip clip = GetClip(_clipId);
        _frameTimer += (float)delta;

        while (_frameTimer >= clip.FrameDuration)
        {
            _frameTimer -= clip.FrameDuration;

            if (_frameIndex < clip.Frames.Length - 1)
            {
                _frameIndex++;
                ApplyFrame();
                continue;
            }

            if (clip.Loop)
            {
                _frameIndex = 0;
                ApplyFrame();
                continue;
            }

            _clipFinished = true;
            if (clip.NextClip.HasValue)
            {
                StartClip(clip.NextClip.Value, _state);
            }

            break;
        }
    }

    public void SetState(MotionState state)
    {
        if (_state == state)
        {
            return;
        }

        switch (state)
        {
            case MotionState.Run:
                StartClip(_state == MotionState.Idle ? ClipId.RunStart : ClipId.RunLoop, state);
                break;
            case MotionState.Jump:
                StartClip(ClipId.Jump, state);
                break;
            case MotionState.Fall:
                StartClip(ClipId.Fall, state);
                break;
            case MotionState.Land:
                StartClip(ClipId.Land, state);
                break;
            default:
                StartClip(ClipId.Idle, MotionState.Idle);
                break;
        }
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

    private void StartClip(ClipId clipId, MotionState state)
    {
        _state = state;
        _clipId = clipId;
        _frameIndex = 0;
        _frameTimer = 0.0f;
        _clipFinished = false;
        ApplyFrame();
    }

    private void ApplyFrame()
    {
        SpriteFrame frame = GetClip(_clipId).Frames[_frameIndex];
        RegionRect = frame.Region;

        float pivotX = FlipH ? frame.Region.Size.X - frame.Pivot.X : frame.Pivot.X;
        Offset = new Vector2(-pivotX, -frame.Pivot.Y);
    }

    private Clip GetClip(ClipId clipId)
    {
        return clipId switch
        {
            ClipId.RunStart => new Clip(_runStartFrames, 0.06f, false, ClipId.RunLoop),
            ClipId.RunLoop => new Clip(_runLoopFrames, 0.06f, true),
            ClipId.Jump => new Clip(_jumpFrames, 0.10f, false),
            ClipId.Fall => new Clip(_fallFrames, 0.12f, false),
            ClipId.Land => new Clip(_landFrames, 0.08f, false),
            _ => new Clip(_idleFrames, 0.20f, true)
        };
    }

    private readonly struct Clip
    {
        public Clip(SpriteFrame[] frames, float frameDuration, bool loop, ClipId? nextClip = null)
        {
            Frames = frames;
            FrameDuration = frameDuration;
            Loop = loop;
            NextClip = nextClip;
        }

        public SpriteFrame[] Frames { get; }

        public float FrameDuration { get; }

        public bool Loop { get; }

        public ClipId? NextClip { get; }
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
