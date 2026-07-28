using Godot;
using Godot.Collections;
using System;

public partial class GridBlock : StaticBody3D
{
	public Vector2I gridPosition;
	public MeshInstance3D meshInstance;
	public GridType type;
	public static Dictionary<GridType, BlockImage> typeToImageDict;

	StandardMaterial3D material;
	float blockSize;

	Array<Ally> allies = new Array<Ally>();
	Array<Enemy> enemies = new Array<Enemy>();

	public override void _Ready()
	{
        base._Ready();
		this.Name = "GridBlock" + gridPosition.ToString();
		meshInstance.Name = "Mesh";
	}

	public GridBlock(GridType type, float blockSize)
	{
		if (typeToImageDict == null) findTexturePaths();
		this.blockSize = blockSize;

		setupMesh();
		setupCollision();
		setType(type);

		InputRayPickable = true;
	}

	public GridBlock(GridType type, Vector2I gridPosition, float blockSize)
	{
		if (typeToImageDict == null) findTexturePaths();
		this.blockSize = blockSize;

		setupMesh();
		setupCollision();
		setType(type);

		this.gridPosition = gridPosition;
		setupLabel(blockSize);

		InputRayPickable = true;
	}

	protected void setupMesh()
	{
		PlaneMesh plane = new PlaneMesh();
		plane.Size = new Vector2(blockSize, blockSize);

		material = new StandardMaterial3D();
		material.AlbedoColor = Colors.White;
		material.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;

		meshInstance = new MeshInstance3D();
		meshInstance.Mesh = plane;
		meshInstance.MaterialOverride = material;
		AddChild(meshInstance);
	}

	protected void setupCollision()
	{
		BoxShape3D box = new BoxShape3D();
		box.Size = new Vector3(blockSize, 0.05f, blockSize);

		CollisionShape3D collision = new CollisionShape3D();
		collision.Shape = box;
		collision.Name = "Collision";
		collision.Position = new Vector3(0, -0.025f, 0);
		AddChild(collision);
	}

	private void setupLabel(float blockSize)
	{
		Label3D label = new Label3D();
		label.Text = ($"{allies.Count} , {enemies.Count}");
		label.PixelSize = 0.01f;
		label.FontSize = (int)(0.3 * blockSize * 100);
		label.Billboard = BaseMaterial3D.BillboardModeEnum.Disabled;
		label.RotationDegrees = new Vector3(-90, 0, 0);
		label.Position = new Vector3(0, 0.01f, 0);
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.VerticalAlignment = VerticalAlignment.Center;
		label.NoDepthTest = true;
		this.AddChild(label);
	}

	public static void findTexturePaths()
	{
		typeToImageDict = new Dictionary<GridType, BlockImage>();
		DirAccess dir = DirAccess.Open(Paths.BlockResourcePath);
		foreach (string file in dir.GetFiles())
		{
			BlockImage image = GD.Load<BlockImage>(Paths.BlockResourcePath + "/" + file);
			if (!Enum.TryParse<GridType>(file.GetBaseName(), out GridType type)) { GD.PrintErr(file + " not recognized as enum"); }
			typeToImageDict.Add(type, image);
		}
	}

	public void setType(GridType type)
	{
		this.type = type;
		if (type == GridType.NULL) { material.AlbedoTexture = null; material.AlbedoColor = Colors.White; return; }
		if (!typeToImageDict.TryGetValue(type, out BlockImage image)) { GD.PrintErr("No texture found for " + type.ToString()); return; }
		material.AlbedoColor = Colors.White;
		material.AlbedoTexture = image.texture;
	}

	public void setHovered(bool hovered)
	{
		material.AlbedoColor = hovered ? Colors.Burlywood : Colors.White;
	}

	
}
