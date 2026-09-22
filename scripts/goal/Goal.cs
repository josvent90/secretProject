using Godot;

public partial class Goal : Area2D
{
	private bool _open;

	public override void _Ready()
	{
		AddToGroup("goal");
		BodyEntered += OnBodyEntered;
		Modulate = new Color(0.55f, 0.55f, 0.62f);
	}

	public void Open()
	{
		_open = true;
		Modulate = Colors.White;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is not Player)
			return;
		if (GetParent() is Level level)
			level.OnGoalReached(_open);
	}
}
