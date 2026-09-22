using Godot;

public partial class Main : Control
{
	public override void _Ready()
	{
		GD.Print("SecretProject listo.");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsEcho())
			return;
		if (@event.IsActionPressed("ui_accept"))
			GetTree().ChangeSceneToFile("res://scenes/level/Level.tscn");
	}
}
