using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class CardQueue : Control
{
	Array<CardGUI> cards = new();
	const float CARD_SPACING = -20;
	const float CARD_ASPECT_RATIO = 2f / 3f;
	public Vector2 discardGlobalPosition;
	public Vector2 drawGlobalPosition;
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
			card.ZIndex = cardCount - i;
			if (i != 0) _ = card.moveToLocal(new Vector2(0, i * cardWidth + CARD_SPACING));
			else _ = card.moveToLocal(Vector2.Zero);
			card.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
			card.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;

			card.CustomMinimumSize = new Vector2(cardWidth, cardHeight);
			card.Size = new Vector2(cardWidth, cardHeight);

			card.Rotation = 0;
			card.PivotOffset = card.Size / 2f;
		}
	}
	public void addCard(CardGUI cardGUI, Vector2 globalPosition)
	{
		cards.Add(cardGUI);
		AddChild(cardGUI);
		cardGUI.GlobalPosition = globalPosition;
		positionCards();
	}
	public async Task removeCard(CardGUI cardGUI)
	{
		cards.Remove(cardGUI);
		await cardGUI.moveToGlobal(discardGlobalPosition, true, true);
		//TODO add discard pile functionality
		RemoveChild(cardGUI);
		positionCards();
	}
}
