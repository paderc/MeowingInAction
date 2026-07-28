using Godot;
using System;

public partial class MapChoice : Button
{
	TextureRect texture;
	Vector2 position;
	public MapChoice()
	{
		this.CustomMinimumSize = new Vector2(50, 50);
		this.Size = new Vector2(50, 50);
		texture = new TextureRect();
		texture.Texture = new PlaceholderTexture2D();
		texture.SetAnchorsPreset(LayoutPreset.FullRect);
		AddChild(texture);
	}
}
