
using Godot;
using System;

public partial class StartMenu : Control
{
	static string startMenuPath = "res://scenes/StartMenu.tscn";

	[Signal]
	public delegate void StartEventHandler();
	[Signal]
	public delegate void ExitEventHandler();
	public LabelAnimHandler<TypewriterLabelComp> labelAnimHandler = new LabelAnimHandler<TypewriterLabelComp>();
	[Export]
	TextureButton playButton;
	[Export]
	TextureButton meowButton;
	[Export]
	TextureButton settingsButton;
	[Export]
	TextureButton exitToDesktopButton;
	public override void _Ready()
	{
		manageButtons();
	}
	void setupLabels()
	{
		Label play = playButton.GetNode<Label>("ResizableLabel");
		labelAnimHandler.addLabel(play);
		Label meow = meowButton.GetNode<Label>("ResizableLabel");
		labelAnimHandler.addLabel(meow);
		Label settings = settingsButton.GetNode<Label>("ResizableLabel");
		labelAnimHandler.addLabel(settings);
		Label exitToDesktop = playButton.GetNode<Label>("ResizableLabel");
		labelAnimHandler.addLabel(exitToDesktop);
		labelAnimHandler.addComponentsToLabels();
	}
	void manageButtons()
	{
		if (playButton == null) { GD.PushError("Button not assigned"); return; };
		if (meowButton == null) { GD.PushError("Button not assigned"); return; }
		if (settingsButton == null) { GD.PushError("Button not assigned"); return; }
		if (exitToDesktopButton == null) {GD.PushError("Button not assigned"); return; }

		playButton.Pressed += () => {
			EmitSignalStart();
		};

		exitToDesktopButton.Pressed += () =>
		{
			EmitSignal(SignalName.Exit);
		};
		setupLabels();
	}
	public static StartMenu create()
	{
		PackedScene scene = GD.Load<PackedScene>(startMenuPath);
		StartMenu startMenu = scene.Instantiate<StartMenu>();
		if (startMenu == null)
		{
			GD.PushError("Did not find start menu scene at " + startMenuPath);
			startMenu.QueueFree();
		}
		return startMenu;
	}

	public override void _Process(double delta)
	{
	}
}
