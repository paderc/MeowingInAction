using Godot;

public partial class EntityGUI : Node3D
{

	Entity.DeadEventHandler deadHandler;
	[Signal]
	public delegate void DeadEventHandler(EntityGUI entityGUI);
	float baseY;
	float oscillationSpeed = 2.0f;
	float oscillation = 0f;
	private bool goUp = true;
	Direction _direction;
	public Direction direction
	{
		get => _direction;
		set { _direction = value; if(moveNode != null) moveNode.correctDirection(_direction); }
	}

	public MoveNode moveNode;
	MeshInstance3D meshInstance;

	bool isDead = false;
	public override void _Ready()
	{
		baseY = Position.Y;
		meshInstance = GetNode<MeshInstance3D>("Muchkin1_002");
		moveNode = MoveNode.create();
		AddChild(moveNode);
		moveNode.correctDirection(direction);
	}


	public override void _Process(double delta)
	{
		float step = oscillationSpeed * (float)delta;

		if (goUp)
		{
			oscillation += step;
			if (oscillation >= 1.0f)
			{
				oscillation = 1.0f;
				goUp = false;
			}
		}
		else
		{
			oscillation -= step;
			if (oscillation <= 0.0f)
			{
				oscillation = 0.0f;
				goUp = true;
			}
		}
		Position = new Vector3(Position.X, baseY + oscillation, Position.Z);
	}
	public void changeColor(Color color)
	{
		StandardMaterial3D material = meshInstance.MaterialOverride as StandardMaterial3D;

		if (material == null)
		{
			var originalMaterial = meshInstance.GetActiveMaterial(0);
			material = originalMaterial.Duplicate() as StandardMaterial3D;
			meshInstance.MaterialOverride = material;
		}

		material.AlbedoColor = color;
	}
	public void resetMaterial()
	{
		changeColor(Colors.White);
	}
	public static EntityGUI getEntityGUI()
	{
		PackedScene scene = GD.Load<PackedScene>(Paths.entityGUIUID);
		EntityGUI entityGUI = scene.Instantiate<EntityGUI>();
		return entityGUI;
	}
	public void die()
	{
		this.QueueFree();
	}
}
