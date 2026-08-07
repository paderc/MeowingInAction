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
        GD.Print("File exists: " + Godot.FileAccess.FileExists(Paths.baseDeckPath));
        return GD.Load<Deck>(Paths.baseDeckPath);
	}
}
