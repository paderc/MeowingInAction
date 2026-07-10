using Godot;
using Godot.Collections;
using System;

public partial class GridBlock : Control
{
	public Vector2I gridPosition;
	public TextureRect textureRect;
	public GridType type;
	public static Dictionary<GridType, string> typeToPathDict;

	public GridBlock(GridType type, float blockSize)
	{
		if (typeToPathDict == null) findTexturePaths();
		this.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		this.SizeFlagsVertical = SizeFlags.ExpandFill;
		this.Size = new Vector2(blockSize, blockSize);

		setupTexture();
		setType(type);

		MouseFilter = MouseFilterEnum.Pass;
	}

	public GridBlock(GridType type, Vector2I gridPosition, float blockSize)
	{
		if (typeToPathDict == null) findTexturePaths();
		this.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		this.SizeFlagsVertical = SizeFlags.ExpandFill;
		this.Size = new Vector2(blockSize, blockSize);

		setupTexture();
		setType(type);

		this.gridPosition = gridPosition;
		setupLabel(blockSize);


		MouseFilter = MouseFilterEnum.Pass;
	}

	protected void setupTexture()
	{
		this.textureRect = new TextureRect();
		textureRect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
		textureRect.StretchMode = TextureRect.StretchModeEnum.Scale;
		textureRect.MouseFilter = MouseFilterEnum.Ignore;
		textureRect.SetAnchorsPreset(LayoutPreset.FullRect);
		AddChild(textureRect);
	}

	private void setupLabel(float blockSize)
	{
		Label label = new Label();
		label.Text = (String)(gridPosition.X + "," + gridPosition.Y);
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.VerticalAlignment = VerticalAlignment.Center;
		label.SetAnchorsPreset(LayoutPreset.FullRect);
		label.AddThemeFontSizeOverride("font_size", (int)(0.6 * blockSize));
		label.MouseFilter = MouseFilterEnum.Ignore;
		this.AddChild(label);
	}

	public static void findTexturePaths()
	{

		Dictionary<string, string> blockTexturePaths = Json.ParseString(FileAccess.GetFileAsString("res://storage/BlockJSON.json")).As<Dictionary<string, string>>();
		
		typeToPathDict = new Dictionary<GridType, string>();
		
		foreach (string key in blockTexturePaths.Keys) {
			if (!blockTexturePaths.TryGetValue(key, out string texturePath)) { GD.PrintErr("Cannot find " + key); }
			if (!Enum.TryParse<GridType>(key, out GridType type)) { GD.PrintErr(key + " not recognized as enum"); }
			typeToPathDict.Add(type, texturePath);
		}

	}

	public void setType(GridType type)
	{
		this.type = type;
		if (type == GridType.NULL) { textureRect.Texture = new PlaceholderTexture2D(); return; }
		if (!typeToPathDict.TryGetValue(type, out string path)) GD.PrintErr("No texture found for " + type.ToString());
		Texture2D texture = (Texture2D)GD.Load(path);
		if (texture == null) GD.PrintErr("Did not find texture at " + path);
		textureRect.Texture = texture; 
	}

	public override void _Ready()
	{
		base._Ready();
		
	}
}
