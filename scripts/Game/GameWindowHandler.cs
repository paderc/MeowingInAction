using Godot;
using System;

public partial class GameWindowHandler : Node
{
	Window window;
	Timer windowModeCooldownTimer = new Timer();
	bool windowModeOnCooldown = false;
	public Vector2I windowedSize = new Vector2I(1280, 720);
	public Vector2I fullscreenSize = new Vector2I(1920, 1080);
	public override void _Ready()
	{
		setupWindow();
		setupWindowModeTimer();
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
	void setupWindowModeTimer()
	{
		float switchCooldownS = 0.2f;
		AddChild(windowModeCooldownTimer);
		windowModeCooldownTimer.Autostart = false;
		windowModeCooldownTimer.WaitTime = switchCooldownS;
		windowModeCooldownTimer.Name = "WindowModeCooldownTimer";
		windowModeCooldownTimer.Timeout += () => windowModeOnCooldown = false;
	}
	void setupWindow()
	{
		window = GetWindow();
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
	}
	public void switchWindowMode()
	{
		if (windowModeOnCooldown) return;
		if (window.Mode == Window.ModeEnum.Fullscreen)
		{
			window.Mode = Window.ModeEnum.Windowed;
			window.Size = windowedSize;
		}
		else
		{
			window.Mode = Window.ModeEnum.Fullscreen;
		}
		windowModeCooldownTimer.Start();
		windowModeOnCooldown = true;
	}
}
