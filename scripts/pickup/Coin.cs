using Godot;

public partial class Coin : Area2D
{
	private bool _taken;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (_taken || body is not Player)
			return;
		if (GetParent() is not Level level)
			return;

		_taken = true;
		Monitoring = false;
		level.CollectCoin();
		QueueFree();
	}
}
