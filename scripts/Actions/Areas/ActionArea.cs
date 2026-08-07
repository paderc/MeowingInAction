using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class ActionArea : Resource
{
	[Export]
	Array<Vector2I> posRelative;
}
