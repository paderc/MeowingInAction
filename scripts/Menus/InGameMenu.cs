using Godot;
using System;

public partial class InGameMenu : Control
{
    static string inGameMenuPath = "res://scenes/InGameMenu.tscn";

	public InGameMenuButtonHandler buttonHandler;
	[Signal]
	public delegate void MenuChangedEventHandler(bool focus);
	bool escapeReleased = true;

	public override void _Ready()
	{
		buttonHandler = GetNode<InGameMenuButtonHandler>("InGameMenuButtonHandler");
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
	public static InGameMenu create()
	{
        PackedScene scene = GD.Load<PackedScene>(inGameMenuPath);
        InGameMenu inGameMenu = scene.Instantiate<InGameMenu>();
        if (inGameMenu == null) GD.PushError("Did not find ingame menu scene at " + inGameMenuPath);
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
        EmitSignal(SignalName.MenuChanged, this.Visible);
    }	
}
