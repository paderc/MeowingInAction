using Godot;
using System;
using System.Reflection.Metadata;

public partial class BattleLoader
{
	public Battle battle;
	Deck deck;
	PackedScene battleScene = GD.Load<PackedScene>(Paths.battleScenePath);
	BattleLoader(Run run)
	{
		deck = run.deck;
	}
	public BattleLoader(Stage stage)
	{
		battle = GridLoader.getForStage(stage);
	}
	public BattleLoader(Stage stage, Run run) : this(run)
	{
		battle = GridLoader.getForStage(stage);
	}
	public BattleLoader(Battle battle)
	{
		this.battle = battle;
	}
	public BattleLoader(Battle battle, Run run) : this(run)
	{
		this.battle = battle;
	}
	public Battle getBattle()
	{
		if (battle == null) { GD.PushError("Battle grid is null, also returning null"); return null; }
		return battle;
	} 

	public Node3D getBattleNode()
	{
		Node3D scene = battleScene.Instantiate<Node3D>();
		Node3D gridSpace = scene.GetNode<Node3D>("GridSpace");
		gridSpace.AddChild(battle);

		Hand hand = scene.GetNode<Hand>("HandLayer/HandSpace");
		foreach (Card card in deck.baseCards)
		{
			hand.addToHand(CardGUI.GetCardGUI(card));
		}
		return scene;
	}
}
