using Godot;
using Godot.Collections;

public partial class Hand : Control
{
	const float MAX_ROTATION = 0.2f;
	const float CARD_SPACING = -20;
	const float CARD_ASPECT_RATIO = 2f / 3f;

	Array<CardGUI> cards = new Array<CardGUI>();

	public override void _Ready()
	{
		Resized += positionCards;
		positionCards();
	}

	public void addToHand(CardGUI card)
	{
		AddChild(card);
		cards.Add(card);
		card.PropagateMaximumSize = true;
		card.draggable.justPickedUp += positionCards;
		card.draggable.justPutDown += positionCards;
		CallDeferred(nameof(positionCards));
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
		float spacing = CARD_SPACING;
		if (naturalTotalWidth > handWidth && cardCount > 1)
		{
			spacing = (handWidth - cardCount * cardWidth) / (cardCount - 1);
		}

		float totalWidth = cardCount * cardWidth + (cardCount - 1) * spacing;
		float startX = handWidth / 2 - totalWidth / 2;

		for (int i = 0; i < cardCount; i++)
		{
			CardGUI card = cards[i];
			card.CustomMinimumSize = Vector2.Zero;
			card.CustomMaximumSize = new Vector2(cardWidth, cardHeight);
			card.Size = new Vector2(cardWidth, cardHeight);

			float t = cardCount == 1 ? 0f : (float)i / (cardCount - 1) * 2.0f - 1.0f;
			float xPos = startX + i * (cardWidth + spacing);

			card.PivotOffset = card.Size / 2f;
			card.Position = new Vector2(xPos, 0);
			card.Rotation = t * MAX_ROTATION;
		}
	}
}
