using Godot;
using Godot.Collections;
using System.Linq;
using System.Threading.Tasks;

public partial class CardActionHandler : Node
{
	public Battle battle;
	public Array<GridBlock> hoveredBlocks = new();
	public Array<GridBlock> previewBlocks = new();

	public CardGUI heldCard;
	
	bool signalsConnected = false;
	public System.Collections.Generic.Queue<System.Tuple<CardGUI, Array<GridBlock>>> cardQueue = new();
	public System.Tuple<CardGUI, Array<GridBlock>> currentPlayed;
	bool isProcessingQueue = false;
	public override void _Ready()
	{
		battle = GetParent<Battle>();
		CallDeferred(nameof(connectSignals));
	}

	void connectSignals()
	{
		if (signalsConnected) return;
		signalsConnected = true;

		battle.battleGrid.HoverUpdated += onHoverUpdated;
		battle.hand.CardPickedUp += onCardPickedUp;
		battle.hand.CardPutDown += onCardPutDown;
	}

	public override void _ExitTree()
	{
		if (!signalsConnected) return;
		signalsConnected = false;

		battle.battleGrid.HoverUpdated -= onHoverUpdated;
		battle.hand.CardPickedUp -= onCardPickedUp;
		battle.hand.CardPutDown -= onCardPutDown;
	}

	void onCardPickedUp(CardGUI cardGUI)
	{
		heldCard = cardGUI;
	}

	async void onCardPutDown(CardGUI cardGUI)
	{
		if (hoveredBlocks != null)
		{
			playCard(cardGUI);	
		}
	}

	void onHoverUpdated(Array<GridBlock> allHovered)
	{
		if (heldCard != null)
		{
			foreach (CardAction action in heldCard.card.actionList)
			{
				action.undoPreview(this);
			}

			hoveredBlocks = allHovered != null
				? new(allHovered)
				: new();

			if (allHovered != null)
			{
				foreach (CardAction action in heldCard.card.actionList)
				{
					action.preview(this);
				}
				previewBlocks = new(hoveredBlocks);
			}
			else
			{
				previewBlocks.Clear();
			}
		}
		else
		{
			hoveredBlocks = allHovered != null
				? new(allHovered)
				: new();
		}
	}
	void playCard(CardGUI cardGUI)
	{
		var targetBlocks = new Array<GridBlock>(hoveredBlocks);
		cardGUI.card.unpreview(this);
		queueToBePlayed(cardGUI, targetBlocks);
		heldCard = null;
	}
	void putCardInQueueVisual(CardGUI cardGUI)
	{
		cardGUI.draggable.canUse = false;
		cardGUI.draggable.snapBack = false;
		battle.hand.removeFromHand(cardGUI);
		battle.cardQueue.addCard(cardGUI);
	}
	void queueToBePlayed(CardGUI cardGUI, Array<GridBlock> blocks)
	{
		cardQueue.Enqueue(new System.Tuple<CardGUI, Array<GridBlock>>(cardGUI, blocks));
		putCardInQueueVisual(cardGUI);
		if (!isProcessingQueue)
		{
			isProcessingQueue = true;
			_ = emptyQueue();
		}
	}
	async Task emptyQueue()
	{
		try
		{
			while (cardQueue.Count > 0)
			{
				currentPlayed = cardQueue.Dequeue();
				await currentPlayed.Item1.card.commitChangesAsync(this);
				battle.cardQueue.removeCard(currentPlayed.Item1);
			}
		}
		finally
		{
			isProcessingQueue = false;
		}
	}
}
