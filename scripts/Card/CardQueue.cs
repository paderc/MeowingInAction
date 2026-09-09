using Godot;
using Godot.Collections;
using System;

public partial class CardQueue : Control
{
	Array<CardGUI> cards = new();
	const float CARD_SPACING = -20;
	const float CARD_ASPECT_RATIO = 2f / 3f;
	void positionCards()
	{
		int cardCount = cards.Count;
		if (cardCount == 0) return;

		float handHeight = Size.Y;
		float cardHeight = handHeight;
		float cardWidth = cardHeight * CARD_ASPECT_RATIO;

		for (int i = 0; i < cardCount; i++)
		{
			CardGUI card = cards[i];
			if (i != 0) card.Position = new Vector2(0, i * cardWidth + CARD_SPACING);
			else card.Position = Vector2.Zero;
			card.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
			card.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;

			card.CustomMinimumSize = new Vector2(cardWidth, cardHeight);
			card.Size = new Vector2(cardWidth, cardHeight);

			card.Rotation = 0;
			card.PivotOffset = card.Size / 2f;
		}
	}
	public void addCard(CardGUI cardGUI)
	{
		cards.Add(cardGUI);
		AddChild(cardGUI);
		positionCards();
	}
	public void removeCard(CardGUI cardGUI)
	{
		cards.Remove(cardGUI);
		RemoveChild(cardGUI);
		positionCards();
	}
}
