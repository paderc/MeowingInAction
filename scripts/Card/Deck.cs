using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class Deck : Resource
{
	[Export]
	public Array<Card> baseCards;
	
	public static Deck LoadBaseDeck()
	{
        return GD.Load<Deck>(Paths.baseDeckPath);
	}
}
