using Godot;
using Godot.Collections;
using System;

public partial class DrawCards : Control
{
	Array<Card> cards = new Array<Card>();
	public DrawCards(Array<Card> deck)
	{
		cards = deck;
	}
	public override void _Ready()
	{

	}
}
