using Godot;
using System;

public partial class Battle : Node3D
{
	public BattleGrid battleGrid;
	CardActionHandler cardActionHandler = new CardActionHandler();
	public Hand hand;
	Battle()
	{
	}
	public Battle(BattleGrid battleGrid)
	{
		this.battleGrid = battleGrid;
		
	}
	public override void _Ready()
	{
		hand = GetNode<Hand>("HandLayer/HandSpace");
		
	}
	public Node3D getBattleNode(Deck deck)
	{
		PackedScene battleScene = GD.Load<PackedScene>(Paths.battleScene);
		Battle battle = battleScene.Instantiate<Battle>();

		Node3D gridSpace = battle.GetNode<Node3D>("BattleGrid/GridSpace");
		gridSpace.AddChild(battleGrid);

		battle.battleGrid = battleGrid;
		battle.AddChild(cardActionHandler);

		hand = battle.GetNode<Hand>("HandLayer/HandSpace");
		setupHand();

		foreach (Card card in deck.baseCards)
		{
			hand.addToHand(CardGUI.GetCardGUI(card));
		}
		return battle;
	}

	void setupHand()
	{
		hand.CardPickedUp += (card) =>
		{
			battleGrid.changeHoverArea(card.area);
		};
		hand.CardPutDown += (card) =>
		{
			battleGrid.resetHoverArea();
		};

	}
}
