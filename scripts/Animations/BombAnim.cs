using Godot;
using System;

public partial class BombAnim : BaseAnim
{
	public override void manageEntry(Entry entry)
	{
		Scale = Vector3.One * (int)entry.amount / 3;
	}
}
