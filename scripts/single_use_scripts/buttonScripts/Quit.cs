using Godot;
using System;

public partial class Quit : NameButton
{
	public override void _Ready()
	{
		Pressed += () => quitGame();
	}

	public void quitGame()
	{
		GetTree().Quit();
	}
}
