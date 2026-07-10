using Godot;
using System;

public partial class Test : Node
{
	Card card = new Card();
	public override void _Ready()
	{
		AddChild(card);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
