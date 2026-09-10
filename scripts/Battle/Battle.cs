using Godot;
using System;

public partial class Battle : Node3D
{
	//TODO add turns
	[Export] Control drawPile, discardPile;
	public BattleGrid battleGrid;
	CardActionHandler cardActionHandler = new CardActionHandler();
	public Deck deck = new Deck();
	public Hand hand;
	public CardQueue cardQueue;

	public static Battle create(BattleGrid battleGrid)
	{
		PackedScene scene = GD.Load<PackedScene>(Paths.battleSceneUID);
		Battle battle = scene.Instantiate<Battle>();
		if (battle == null)
		{
			GD.PushError("Did not find battle scene at " + Paths.battleSceneUID);
			battle.QueueFree();
		}
		battle.battleGrid = battleGrid;
		return battle;
	}

	public override void _Ready()
	{
		hand = GetNode<Hand>("HandLayer/HandSpace");
		setupHand();
		Node3D gridSpace = GetNode<Node3D>("BattleGrid/GridSpace");
		gridSpace.AddChild(battleGrid);
		AddChild(cardActionHandler);
		cardQueue = GetNode<CardQueue>("%CardQueue");
		cardQueue.discardGlobalPosition = discardPile.GlobalPosition;
		cardQueue.drawGlobalPosition = discardPile.GlobalPosition;
	}

	void setupHand()
	{
		hand.CardPickedUp += (cardGUI) => battleGrid.changeHoverArea(cardGUI.card.area);
		hand.CardPutDown += (cardGUI) => battleGrid.resetHoverArea();

		//TODO add draw pile first
		foreach (Card card in Run.deck.baseCards)
		{
			hand.addToHand(CardGUI.GetCardGUI(card));
		}
	}
}
