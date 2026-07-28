using Godot;
using System;

[GlobalClass]
public partial class MapRes : Resource
{
	[Export]
	public Texture2D texture;
	[Export]
	public int levelCount;
	
}
