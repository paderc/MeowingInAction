using Godot;
using System;

public partial class Run
{
	Deck playerDeck;
	Stage currentStage;
	BattleGrid battleGrid;

	public Run()
	{
		currentStage = TempConstants.StartStage;
	}
}
