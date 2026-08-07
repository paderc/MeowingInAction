using Godot;
using Godot.Collections;
using System;

public partial class CardActionHandler : Node
{
	public Battle battle;
	public Array<GridBlock> hovered;


	public override void _Ready()
	{
		battle = GetParent<Battle>();
		CallDeferred(nameof(connectSignals));
	}

	void connectSignals()
	{
		battle.battleGrid.HoverUpdated += (allHovered) =>
		{
			hovered = allHovered;
		};
		battle.hand.CardPutDown += (card) =>
		{
			if (hovered != null)
			{
				card.doActions(this);
			}
		};
	}
}
