using Godot;
using System;

public partial class CardActionHandler : Node
{
	public Battle battle;
	public GridBlock currentHovered;


	public override void _Ready()
	{
		battle = GetParent<Battle>();
		CallDeferred(nameof(connectSignals));
	}

	void connectSignals()
	{
		battle.battleGrid.HoverUpdated += (block) =>
		{  
			currentHovered = block; 
		};
		battle.hand.CardPlayed += (card) =>
		{
			card.doActions(this);
		};
	}
}
