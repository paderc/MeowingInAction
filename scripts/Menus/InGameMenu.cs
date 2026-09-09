using Godot;
using Godot.Collections;
using System;

public partial class InGameMenu : Control
{
	static string inGameMenuPath = "res://scenes/InGameMenu.tscn";
	LabelAnimHandler<GlitchLabelComp> labelAnimHandler = new LabelAnimHandler<GlitchLabelComp>();
	[Signal]
	public delegate void MenuOpenedEventHandler(bool focus);
	[Signal]
	public delegate void ResumeEventHandler();
	[Signal]
	public delegate void SettingsOpenedEventHandler();
	[Signal]
	public delegate void ExitedEventHandler();
	[Signal]
	public delegate void ExitedToDesktopEventHandler();
	[Export]
	TextureButton resumeButton;
	[Export]
	TextureButton settingsButton;
	[Export]
	TextureButton exitButton;
	[Export]
	TextureButton exitToDesktopButton;
	bool escapeReleased = true;

	public override void _Ready()
	{
		if (resumeButton == null) Logger.Warning("Button not assigned");
		else resumeButton.Pressed += () => EmitSignal(SignalName.Resume);
		
		if (settingsButton == null) Logger.Warning("Button not assigned");
		else settingsButton.Pressed += () => EmitSignal(SignalName.SettingsOpened);

		if (exitButton == null) Logger.Warning("Button not assigned");
		else exitButton.Pressed += () => EmitSignal(SignalName.Exited);

		if (exitToDesktopButton == null) Logger.Warning("Button not assigned");
		else exitToDesktopButton.Pressed += () => EmitSignal(SignalName.ExitedToDesktop);
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent)
		{
			if (keyEvent.Keycode == Key.Escape)
			{
				changeVisibility();
			}
			if (keyEvent.IsActionReleased("ui_cancel"))
			{
				escapeReleased = true;
			}
		}
	}
	void addLabels()
	{
		Label resume = GetNode<Label>("ButtonArray/Resume/ResizableLabel");
		labelAnimHandler.addLabel(resume);
		Label meow = GetNode<Label>("ButtonArray/Meow/ResizableLabel");
		labelAnimHandler.addLabel(meow);
		Label settings = GetNode<Label>("ButtonArray/Settings/ResizableLabel");
		labelAnimHandler.addLabel(settings);
		Label exit = GetNode<Label>("ButtonArray/Exit/ResizableLabel");
		labelAnimHandler.addLabel(exit);
		Label exitToDesktop = GetNode<Label>("ButtonArray/ExitToDesktop/ResizableLabel");
		labelAnimHandler.addLabel(resume);
	}
	
	public static InGameMenu create()
	{
		PackedScene scene = GD.Load<PackedScene>(inGameMenuPath);
		InGameMenu inGameMenu = scene.Instantiate<InGameMenu>();
		if (inGameMenu == null)
		{
			GD.PushError("Did not find ingame menu scene at " + inGameMenuPath);
			inGameMenu.QueueFree();
		}
		return inGameMenu;
	}
	public void turnOffByResumeButton()
	{
		this.Visible = false;
	}
	void changeVisibility()
	{
		if (!escapeReleased) return;
		escapeReleased = false;
		this.Visible = !this.Visible;
		EmitSignal(SignalName.MenuOpened, this.Visible);
	}	
}
