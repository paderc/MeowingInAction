using Godot;
using Godot.Collections;

public partial class Hand : Control
{
	[Signal]
	public delegate void CardPlayedEventHandler(Card card);
	[Signal]
	public delegate void CardPickedUpEventHandler(Card card);
	[Signal]
	public delegate void CardPutDownEventHandler(Card card);
	const float MAX_ROTATION = 0.1f;
	const float CARD_SPACING = -20;
	const float CARD_ASPECT_RATIO = 2f / 3f;

	Array<CardGUI> cards = new Array<CardGUI>();

	bool canPlayCard = true;
	public CardGUI currentHeld;

	public override void _Ready()
	{
		Control cardDropSpace = GetNode<Control>("../CardDropSpace");
		cardDropSpace.MouseEntered += () => canPlayCard = true;
		cardDropSpace.MouseExited += () => canPlayCard = true;
		
		Resized += positionCards;
		positionCards();
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent)
		{
			if (keyEvent.Keycode == Key.A)
			{
				positionCards();
			}
		}
	}
	public void forceHeldDown()
	{
		if (currentHeld != null)
		{
			canPlayCard = false;
			currentHeld.draggable.silentPutDown();
		}
	}
	public void addToHand(CardGUI cardGUI)
	{
		AddChild(cardGUI);
		cards.Add(cardGUI);
		cardGUI.draggable.justPickedUp += () =>
		{
			currentHeld = cardGUI;
			cardGUI.makeTransparent();
			cards.Remove(cardGUI);
			positionCards();
			EmitSignalCardPickedUp(cardGUI.card);
		};
		cardGUI.draggable.justPutDown += () =>
		{
			if (canPlayCard)
			{
				playCard(cardGUI.card);
				return;
			}
			currentHeld = null;
			cardGUI.restoreTransparency();
			cards.Add(cardGUI);
			positionCards();
			EmitSignalCardPutDown(cardGUI.card);
		};
		CallDeferred(nameof(positionCards));
	}
	void playCard(Card card)
	{
		EmitSignalCardPlayed(card);
		currentHeld = null;
	}
	public void removeFromHand(CardGUI card)
	{
		cards.Remove(card);
		card.draggable.justPickedUp -= positionCards;
		card.draggable.justPutDown -= positionCards;
		RemoveChild(card);
		CallDeferred(nameof(positionCards));
	}

	void positionCards()
	{
		int cardCount = cards.Count;
		if (cardCount == 0) return;

		float handWidth = Size.X;

		float cardHeight = Size.Y;
		float cardWidth = cardHeight * CARD_ASPECT_RATIO;

		float naturalTotalWidth = cardCount * cardWidth + (cardCount - 1) * CARD_SPACING;

		float totalWidth = cardCount * cardWidth + (cardCount - 1) * CARD_SPACING;
		float startX = handWidth / 2 - totalWidth / 2;

		for (int i = 0; i < cardCount; i++)
		{
			CardGUI card = cards[i];
			card.CustomMinimumSize = Vector2.Zero;
			card.CustomMaximumSize = new Vector2(cardWidth, cardHeight);
			card.Size = new Vector2(cardWidth, cardHeight);

			float t = cardCount == 1 ? 0f : (float)i / (cardCount - 1) * 2.0f - 1.0f;
			float xPos = startX + i * (cardWidth + CARD_SPACING);

			card.PivotOffset = card.Size / 2f;
			card.Position = new Vector2(xPos, 0);
			card.Rotation = t * MAX_ROTATION;

			card.ZIndex = i;
		}
	}
}
