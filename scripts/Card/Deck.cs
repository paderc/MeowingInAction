using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class Deck : Resource
{
	static string baseDeckPath = "res://resources/baseDecks/BaseDeck.tres";
	[Export]
	public Array<Card> baseCards;
	public Deck()
	{
		
	}
	public static Deck LoadBaseDeck()
	{
		const string path = "res://resources/baseDecks/BaseDeck.tres";
		return GD.Load<Deck>(path);
	}
}
