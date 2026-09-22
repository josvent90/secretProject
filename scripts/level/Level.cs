using Godot;

public partial class Level : Node2D
{
	public const int TileSize = 16;
	public const int CoinsNeeded = 3;

	// . vacío  # tierra  = verde  % piedra  o moneda  P inicio  D puerta
	// La hoja de Kenney es 16×16 con 1 px de separación: el atlas usa índice, no píxel.
	private static readonly string[] Map =
	{
		"........................................",
		"........................................",
		"........................................",
		"........................................",
		"..................o.....................",
		"...............=======..................",
		"........................................",
		"..........#####.......#####.............",
		"........o..................o............",
		"......#####.............#######.........",
		"P....................................D..",
		"############..####################%%%%%%",
	};

	private static readonly Vector2I DirtTop = new(1, 28);
	private static readonly Vector2I DirtFill = new(1, 29);
	private static readonly Vector2I Green = new(10, 29);
	private static readonly Vector2I Stone = new(4, 29);

	private TileMapLayer _ground;
	private Player _player;
	private Label _coinsLabel;
	private Label _statusLabel;
	private Vector2 _spawn;
	private int _coins;
	private int _spawnedCoins;
	private bool _won;
	private int _sourceId;

	public override void _Ready()
	{
		AddToGroup("level");
		_ground = GetNode<TileMapLayer>("Ground");
		_coinsLabel = GetNode<Label>("Hud/Coins");
		_statusLabel = GetNode<Label>("Hud/Status");
		_statusLabel.Text = "";

		if (!BuildTileSet())
			return;

		Paint();
		_coinsLabel.Text = $"0/{CoinsNeeded}";
		GD.Print("Nivel cargado.");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_won || _player == null)
			return;
		if (_player.GlobalPosition.Y > Map.Length * TileSize + 32f)
			_player.ResetMotion(_spawn);
	}

	public void CollectCoin()
	{
		if (_won)
			return;

		_coins++;
		_coinsLabel.Text = $"{_coins}/{CoinsNeeded}";
		if (_coins < CoinsNeeded)
		{
			_statusLabel.Text = "";
			return;
		}

		_statusLabel.Text = "La puerta está abierta.";
		foreach (Node node in GetTree().GetNodesInGroup("goal"))
		{
			if (node is Goal goal)
				goal.Open();
		}
	}

	public void OnGoalReached(bool open)
	{
		if (_won)
			return;
		if (!open)
		{
			_statusLabel.Text = "Faltan monedas.";
			return;
		}

		_won = true;
		_statusLabel.Text = "Nivel listo.";
		_player.Freeze();
	}

	private bool BuildTileSet()
	{
		var texture = GD.Load<Texture2D>("res://assets/Spritesheet/roguelikeSheet_transparent.png");
		if (texture == null)
		{
			GD.PushError("No se encontró la hoja de sprites.");
			return false;
		}

		var tileSet = new TileSet();
		tileSet.TileSize = new Vector2I(TileSize, TileSize);
		tileSet.AddPhysicsLayer();
		tileSet.SetPhysicsLayerCollisionLayer(0, 1);
		tileSet.SetPhysicsLayerCollisionMask(0, 1);

		var atlas = new TileSetAtlasSource();
		atlas.Texture = texture;
		atlas.TextureRegionSize = new Vector2I(TileSize, TileSize);
		atlas.Margins = Vector2I.Zero;
		atlas.Separation = new Vector2I(1, 1);
		atlas.UseTexturePadding = false;
		_sourceId = tileSet.AddSource(atlas);

		AddSolidTile(atlas, DirtTop);
		AddSolidTile(atlas, DirtFill);
		AddSolidTile(atlas, Green);
		AddSolidTile(atlas, Stone);

		_ground.TileSet = tileSet;
		_ground.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;
		return true;
	}

	private static void AddSolidTile(TileSetAtlasSource atlas, Vector2I coords)
	{
		atlas.CreateTile(coords);
		TileData data = atlas.GetTileData(coords, 0);
		data.AddCollisionPolygon(0);
		// El polígono del tile está centrado en la celda, no en la esquina.
		float half = TileSize * 0.5f;
		data.SetCollisionPolygonPoints(0, 0, new Vector2[]
		{
			new Vector2(-half, -half),
			new Vector2(half, -half),
			new Vector2(half, half),
			new Vector2(-half, half),
		});
	}

	private void Paint()
	{
		int width = Map[0].Length;
		var playerScene = GD.Load<PackedScene>("res://scenes/player/Player.tscn");
		var coinScene = GD.Load<PackedScene>("res://scenes/pickup/Coin.tscn");
		var goalScene = GD.Load<PackedScene>("res://scenes/goal/Goal.tscn");

		for (int y = 0; y < Map.Length; y++)
		{
			string row = Map[y];
			if (row.Length != width)
			{
				GD.PushError($"La fila {y} mide {row.Length} y la primera mide {width}.");
				continue;
			}

			for (int x = 0; x < width; x++)
			{
				switch (row[x])
				{
					case '#':
					case '=':
					case '%':
						_ground.SetCell(new Vector2I(x, y), _sourceId, AtlasFor(row[x], x, y));
						break;
					case 'o':
						Spawn(coinScene, x, y, true);
						_spawnedCoins++;
						break;
					case 'D':
						Spawn(goalScene, x, y, true);
						break;
					case 'P':
						_spawn = new Vector2(x * TileSize + 8, (y + 1) * TileSize);
						break;
				}
			}
		}

		if (_spawnedCoins != CoinsNeeded)
			GD.PushError($"El mapa tiene {_spawnedCoins} monedas; se esperaban {CoinsNeeded}.");

		_player = playerScene.Instantiate<Player>();
		_player.Position = _spawn;
		AddChild(_player);
		_player.ConfigureCamera(width * TileSize, Map.Length * TileSize);
	}

	private void Spawn(PackedScene scene, int x, int y, bool center)
	{
		Node2D node = scene.Instantiate<Node2D>();
		node.Position = center
			? new Vector2(x * TileSize + 8, y * TileSize + 8)
			: new Vector2(x * TileSize, y * TileSize);
		AddChild(node);
	}

	private static bool IsSolid(char cell)
	{
		return cell is '#' or '=' or '%';
	}

	private Vector2I AtlasFor(char cell, int x, int y)
	{
		bool exposed = y == 0 || !IsSolid(Map[y - 1][x]);
		if (cell == '=')
			return Green;
		if (cell == '%')
			return Stone;
		return exposed ? DirtTop : DirtFill;
	}
}
