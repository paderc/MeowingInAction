using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class GridBlock : StaticBody3D
{
	public Vector2I gridPosition;
	public MeshInstance3D meshInstance;
	public GridType type;
	public static Dictionary<GridType, BlockImage> typeToImageDict;
	Label3D label;

	StandardMaterial3D material;
	public float blockSize;

	public EntityHandler entityHandler;


	public override void _Ready()
	{
		this.Name = "GridBlock" + gridPosition.ToString();
		meshInstance.Name = "Mesh";
		entityHandler = new EntityHandler(this);
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
		box.Size = new Vector3(blockSize, 0, blockSize);

		CollisionShape3D collision = new CollisionShape3D();
		collision.Shape = box;
		collision.Name = "Collision";
		collision.Position = new Vector3(0, 0.05f, 0);
		AddChild(collision);
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
