using Godot;
using System;

public partial class InGameMenuButtonHandler : Node
{
	[Signal]
	public delegate void ResumeEventHandler();
	[Signal]
	public delegate void SettingsOpenedEventHandler();
	[Signal]
	public delegate void ExitedEventHandler();
	[Signal]
	public delegate void ExitedToDesktopEventHandler();
	public override void _Ready()
	{
		TextureButton resumeButton = GetNode<TextureButton>("../ButtonArray/Resume");
		resumeButton.Pressed += () => EmitSignal(SignalName.Resume);

		TextureButton settingsButton = GetNode<TextureButton>("../ButtonArray/Settings");
		settingsButton.Pressed += () => EmitSignal(SignalName.SettingsOpened);

		TextureButton exitButton = GetNode<TextureButton>("../ButtonArray/Exit");
		exitButton.Pressed += () => EmitSignal(SignalName.Exited);

		TextureButton exitToDesktopButton = GetNode<TextureButton>("../ButtonArray/ExitToDesktop");
		exitToDesktopButton.Pressed += () => EmitSignal(SignalName.ExitedToDesktop);
	}
}
