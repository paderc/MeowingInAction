using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class CardActionHandler : Node
{
	public Battle battle;
	public Array<GridBlock> hoveredBlocks = new Array<GridBlock>();
	public Array<GridBlock> previewBlocks = new Array<GridBlock>();
	
	public CardGUI heldCard;
	Queue<CardGUI> toBePlayed;
	public override void _Ready()
	{
		battle = GetParent<Battle>();
		CallDeferred(nameof(connectSignals));
	}

	void connectSignals()
	{
		battle.battleGrid.HoverUpdated += (allHovered) =>
		{
			onHoverUpdated(allHovered);
		};
		battle.hand.CardPickedUp += (cardGUI) =>
		{
			heldCard = cardGUI;
		};
		battle.hand.CardPutDown += (cardGUI) =>
		{
			if (hoveredBlocks != null)
			{
				cardGUI.card.doActions(this);
				heldCard = null;
			}
		};
	}
	void onHoverUpdated(Array<GridBlock> allHovered)
	{
		foreach (GridBlock block in previewBlocks)
		{
			block.entityHandler.clearPreviewEntities();
		}
		previewBlocks.Clear();
		hoveredBlocks = new Array<GridBlock>(allHovered);
		if (heldCard != null)
		{
			heldCard.card.preview(this);
			previewBlocks = new Array<GridBlock>(hoveredBlocks);
		}
	}
	void queueToBePlayed(CardGUI cardGUI)
	{
		toBePlayed.Enqueue(cardGUI);
	}
}
