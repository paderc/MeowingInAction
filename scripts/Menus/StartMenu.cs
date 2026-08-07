using Godot;
using System;

public partial class StartMenu : Control
{
	static string startMenuPath = "res://scenes/StartMenu.tscn";

	[Signal]
	public delegate void StartEventHandler();
	[Signal]
	public delegate void ExitEventHandler();
	public override void _Ready()
	{
		TextureButton playButton = GetNode<TextureButton>("ButtonArray/Play");
		if (playButton == null)
		{
			return;
		}
		playButton.Pressed += () => {
			EmitSignalStart();
		};

		TextureButton exitToDesktopButton = GetNode<TextureButton>("ButtonArray/Exit");
		exitToDesktopButton.Pressed += () =>
		{
			EmitSignal(SignalName.Exit);
		};
	}

	public static StartMenu create()
	{
		PackedScene scene = GD.Load<PackedScene>(startMenuPath);
		StartMenu startMenu = scene.Instantiate<StartMenu>();
		if (startMenu == null) GD.PushError("Did not find start menu scene at " + startMenuPath);
		return startMenu;
	}

	public override void _Process(double delta)
	{
	}
}
