using Godot;
using System;

public partial class SerializableBlock : Resource
{
	public SerializableBlock()
	{
	}
	[Export]
	public GridType type;
	[Export]
	public Vector2I position;

}
