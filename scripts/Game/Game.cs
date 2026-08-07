using Godot;
using System;

public partial class Game : Node
{
	StateHandler mainLayerHandler;
	StateHandler popupLayerHandler;

	StartMenu startMenu;
	InGameMenu inGameMenu;

	Map map;

	Node3D battleNode;
	Battle battle;
	Run run;

	public override void _Ready()
	{
		CallDeferred(nameof(setup));
	}

	void setup()
	{
		mainLayerHandler = new StateHandler(GetNode<CanvasLayer>("MainLayer"));
		popupLayerHandler = new StateHandler(GetNode<CanvasLayer>("PopupLayer"));
		this.ProcessMode = ProcessModeEnum.Always;

		setupStartMenu();
		setupInGameMenu();
	}
	void setupStartMenu()
	{
		startMenu = StartMenu.create();
		mainLayerHandler.switchCurrent(startMenu);
		startMenu.Start += startRun;
		startMenu.Exit += leaveGame;
	}
	void setupInGameMenu()
	{
		inGameMenu = InGameMenu.create();
		popupLayerHandler.switchCurrent(inGameMenu);
		inGameMenu.ProcessMode = ProcessModeEnum.Disabled;


		inGameMenu.buttonHandler.Resume += () => inGameMenu.turnOffByResumeButton();
		inGameMenu.buttonHandler.Exited += () => 
		{
			mainLayerHandler.switchCurrent(startMenu);
			inGameMenu.Visible = false;
			inGameMenu.ProcessMode = ProcessModeEnum.Disabled;
		};
		inGameMenu.buttonHandler.ExitedToDesktop += () => GetTree().Quit();
	}
	void startRun()
	{
		run = new Run();
		map = new Map(run.currentStage);

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
		battle = new Battle(BattleGrid.getBattleGrid(run.currentStage));
		battleNode = battle.getBattleNode(run.deck);
		
		Hand hand = battleNode.GetNode<Hand>("HandLayer/HandSpace");
		inGameMenu.MenuOpened += (focus) => hand.forceHeldDown();
		
		mainLayerHandler.switchCurrent(battleNode);
	}
	void leaveGame()
	{
		GetTree().Quit();
	}
	
	
}
