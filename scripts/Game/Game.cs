using Godot;
using System;

public partial class Game : Node
{
	StateHandler mainLayerHandler;
	StateHandler popupLayerHandler;

	StartMenu startMenu;
	InGameMenu inGameMenu;

	Map map;

	Battle battle;

	public override void _Ready()
	{
		CallDeferred(nameof(setup));
	}

	void setup()
	{
		mainLayerHandler = new StateHandler(GetNode<CanvasLayer>("MainLayer"));
		popupLayerHandler = new StateHandler(GetNode<CanvasLayer>("PopupLayer"));
		this.ProcessMode = ProcessModeEnum.Always;

		switchToStartMenu();
	}
	void setupStartMenu()
	{
		startMenu = StartMenu.create();
		mainLayerHandler.switchCurrent(startMenu, startMenu.labelAnimHandler);
		startMenu.Start += startRun;
		startMenu.Exit += leaveGame;
	}
	void setupInGameMenu()
	{
		inGameMenu = InGameMenu.create();
		popupLayerHandler.switchCurrent(inGameMenu);
		inGameMenu.ProcessMode = ProcessModeEnum.Disabled;


		inGameMenu.Resume += inGameMenu.turnOffByResumeButton;
		inGameMenu.Exited += switchToStartMenu;
		inGameMenu.ExitedToDesktop += () => GetTree().Quit();
	}
	void switchToStartMenu()
	{
		setupStartMenu();
		setupInGameMenu();
	}
	void startRun()
	{
		map = new Map(Run.currentStage);

		mainLayerHandler.switchCurrent(map);
		inGameMenu.Visible = false;
		inGameMenu.ProcessMode = ProcessModeEnum.Always;
		CallDeferred(nameof(connectMapChoices));
	}
	void connectMapChoices()
	{
		if (map == null) return;
		foreach (var choice in map.choices)
		{
			choice.Pressed += () => startBattle();
		}
	}
	void startBattle()
	{
		ShaderWarmup.WarmUpAnimations(this);
		ShaderWarmup.WarmUpEntityGUI(this);
		if (!Run.started) Run.startNew(map.stage);
		battle = Battle.create(BattleGrid.getBattleGrid(Run.currentStage));
		
		Hand hand = battle.GetNode<Hand>("HandLayer/HandSpace");
		inGameMenu.MenuOpened += (focus) => hand.forceHeldDown();
		
		mainLayerHandler.switchCurrent(battle);
	}
	void leaveGame()
	{
		GetTree().Quit();
	}
	
	
}
