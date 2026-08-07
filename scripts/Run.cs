using Godot;
using System;

public partial class Run
{
	public Deck deck;
	public Stage currentStage;
	public Battle battle;

	public Run()
	{
        GD.Print("before LoadBaseDeck");
        deck = Deck.LoadBaseDeck();
        GD.Print("after LoadBaseDeck, before StartStage");
        currentStage = TempConstants.StartStage;
        GD.Print("after StartStage");
    }
}
