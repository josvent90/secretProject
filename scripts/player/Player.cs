using Godot;

public partial class Player : CharacterBody2D
{
	// ~52 px de salto: alcanza una plataforma dos tiles (32 px) más arriba.
	[Export] public float Speed = 120f;
	[Export] public float JumpVelocity = -340f;
	[Export] public float Gravity = 1100f;
	[Export] public float CoyoteTime = 0.08f;
	[Export] public float JumpBuffer = 0.1f;

	private Sprite2D _sprite;
	private float _coyote;
	private float _buffer;
	private float _walk;
	private bool _frozen;

	public override void _Ready()
	{
		EnsureActions();
		_sprite = GetNode<Sprite2D>("Sprite2D");
		FloorSnapLength = 4f;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_frozen)
		{
			Velocity = Vector2.Zero;
			return;
		}

		float dt = (float)delta;
		Vector2 velocity = Velocity;

		if (IsOnFloor())
			_coyote = CoyoteTime;
		else
		{
			velocity.Y += Gravity * dt;
			if (velocity.Y > 420f)
				velocity.Y = 420f;
			_coyote -= dt;
		}

		if (Input.IsActionJustPressed("jump"))
			_buffer = JumpBuffer;
		else if (_buffer > 0f)
			_buffer -= dt;

		float direction = Input.GetAxis("move_left", "move_right");
		velocity.X = direction * Speed;

		if (_buffer > 0f && _coyote > 0f)
		{
			velocity.Y = JumpVelocity;
			_buffer = 0f;
			_coyote = 0f;
		}
		else if (Input.IsActionJustReleased("jump") && velocity.Y < 0f)
		{
			velocity.Y *= 0.5f;
		}

		Velocity = velocity;
		MoveAndSlide();

		if (direction < 0f)
			_sprite.FlipH = true;
		else if (direction > 0f)
			_sprite.FlipH = false;

		if (direction != 0f && IsOnFloor())
		{
			_walk += dt;
			_sprite.Frame = (int)(_walk * 10f) % 2;
		}
		else
		{
			_sprite.Frame = 0;
		}
	}

	public void ConfigureCamera(int pixelWidth, int pixelHeight)
	{
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		camera.LimitLeft = 0;
		camera.LimitTop = 0;
		camera.LimitRight = pixelWidth;
		camera.LimitBottom = pixelHeight;
	}

	public void ResetMotion(Vector2 feetPosition)
	{
		GlobalPosition = feetPosition;
		Velocity = Vector2.Zero;
		_coyote = 0f;
		_buffer = 0f;
		_sprite.Frame = 0;
	}

	public void Freeze()
	{
		_frozen = true;
		Velocity = Vector2.Zero;
		_sprite.Frame = 0;
	}

	private static void EnsureActions()
	{
		Bind("move_left", Key.A, Key.Left);
		Bind("move_right", Key.D, Key.Right);
		Bind("jump", Key.Space, Key.W, Key.Up);
	}

	private static void Bind(string action, params Key[] keys)
	{
		if (!InputMap.HasAction(action))
			InputMap.AddAction(action);
		if (InputMap.ActionGetEvents(action).Count > 0)
			return;

		foreach (Key key in keys)
		{
			var ev = new InputEventKey();
			ev.PhysicalKeycode = key;
			InputMap.ActionAddEvent(action, ev);
		}
	}
}
