using Godot;
using System;
using System.Collections.Generic;
using System.Transactions;

public partial class Card : Control
{
	TextureRect cardTexture = new TextureRect();
	Label text;
	List<Action> actionList = new List<Action>();
	int cost;
	
	public Card()
	{
		cardTexture.AddChild(text);
		setupTexture("res://resources/images/backgroundNoiseTexture.res");
	}

	public void setupTexture(String texturePath)
	{
		Resource basicTexture = GD.Load(texturePath);
		if (basicTexture == null) throw new ArgumentNullException("Null texture");
		cardTexture.Texture = (Texture2D)basicTexture;
		cardTexture.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
		CustomMinimumSize = new Vector2(250, 500);
		cardTexture.SetAnchorsPreset(LayoutPreset.FullRect);
		cardTexture.Visible = true;
	}
}
