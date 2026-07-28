using Godot;
using System;

public partial class Run
{
	public Deck deck;
	public Stage currentStage;
	public Battle battle;

	public Run()
	{
		deck = Deck.LoadBaseDeck();
		currentStage = TempConstants.StartStage;
	}
}
