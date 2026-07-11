using Godot;
using Godot.Collections;
using System;

public partial class GridBlock : StaticBody3D
{
	public Vector2I gridPosition;
	public MeshInstance3D meshInstance;
	public GridType type;
	public static Dictionary<GridType, string> typeToPathDict;

	StandardMaterial3D material;
	float blockSize;

	public GridBlock(GridType type, float blockSize)
	{
		if (typeToPathDict == null) findTexturePaths();
		this.blockSize = blockSize;

		setupMesh();
		setupCollision();
		setType(type);

		InputRayPickable = true;
	}

	public GridBlock(GridType type, Vector2I gridPosition, float blockSize)
	{
		if (typeToPathDict == null) findTexturePaths();
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
		collision.Position = new Vector3(0, -0.025f, 0);
		AddChild(collision);
	}

	private void setupLabel(float blockSize)
	{
		Label3D label = new Label3D();
		label.Text = (String)(gridPosition.X + "," + gridPosition.Y);
		label.PixelSize = 0.01f;
		if (gridPosition.X < 10 && gridPosition.Y < 10) label.FontSize = (int)(0.6 * blockSize * 100);
		else if (gridPosition.X > 9 || gridPosition.Y > 9) label.FontSize = (int)(0.35 * blockSize * 100);
		label.Billboard = BaseMaterial3D.BillboardModeEnum.Disabled;
		label.RotationDegrees = new Vector3(-90, 0, 0); // lie flat, facing up
		label.Position = new Vector3(0, 0.01f, 0);
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.VerticalAlignment = VerticalAlignment.Center;
		label.NoDepthTest = true;
		this.AddChild(label);
	}

	public static void findTexturePaths()
	{
		Dictionary<string, string> blockTexturePaths = Json.ParseString(FileAccess.GetFileAsString("res://storage/BlockJSON.json")).As<Dictionary<string, string>>();

		typeToPathDict = new Dictionary<GridType, string>();

		foreach (string key in blockTexturePaths.Keys)
		{
			if (!blockTexturePaths.TryGetValue(key, out string texturePath)) { GD.PrintErr("Cannot find " + key); }
			if (!Enum.TryParse<GridType>(key, out GridType type)) { GD.PrintErr(key + " not recognized as enum"); }
			typeToPathDict.Add(type, texturePath);
		}
	}

	public void setType(GridType type)
	{
		this.type = type;
		if (type == GridType.NULL) { material.AlbedoTexture = null; material.AlbedoColor = Colors.Gray; return; }
		if (!typeToPathDict.TryGetValue(type, out string path)) { GD.PrintErr("No texture found for " + type.ToString()); return; }
		Texture2D texture = (Texture2D)GD.Load(path);
		if (texture == null) GD.PrintErr("Did not find texture at " + path);
		material.AlbedoColor = Colors.White;
		material.AlbedoTexture = texture;
	}

	public void setHovered(bool hovered)
	{
		material.EmissionEnabled = hovered;
		material.Emission = hovered ? new Color(0.4f, 0.6f, 1f) : Colors.Black;
		material.EmissionEnergyMultiplier = hovered ? 0.6f : 0f;
	}

	public override void _Ready()
	{
		base._Ready();
	}
}
