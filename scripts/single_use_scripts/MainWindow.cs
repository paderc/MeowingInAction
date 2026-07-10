using Godot;
using System;

public partial class MainWindow : Control
{
	public Vector2I windowedSize = new Vector2I(1280, 720);
	public Vector2I fullscreenSize = new Vector2I(1920, 1080);
	Window window;
	Control menu;

	public override void _Ready()
	{
		window = GetWindow();
		PackedScene startMenuScene = GD.Load<PackedScene>("res://scenes/StartScenes/StartScreen.tscn");
		menu = startMenuScene.Instantiate<Control>();
		AddChild(menu);
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent)
		{
			if (keyEvent.Keycode == Key.F4)
			{
				switchWindowMode();
			}
		}
	}
	public void switchWindowMode() {
		if (window.Mode == Window.ModeEnum.Fullscreen)
		{
			window.Mode = Window.ModeEnum.Windowed;
			window.Size = windowedSize;
		}
		else
		{
			window.Mode = Window.ModeEnum.Fullscreen;
		}
	}
}
