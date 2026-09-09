using Godot;
using System;

public static partial class Run
{
	public static Deck deck;
	public static Stage currentStage;
	public static Battle battle;
	public static bool started = false;
	public static void startNew(Stage stage)
	{
		started = true;
		currentStage = stage;
		deck = GD.Load<Deck>(Paths.baseDeckUID);
		GD.PrintErr("Using fallback deck in Run");
	}
	public static void startNew(Stage stage, Deck deck)
	{
		started = true;
		currentStage = stage;
		Run.deck = deck;
	}
	public static void end()
	{
		started = false;
		deck = null;
	}
}
