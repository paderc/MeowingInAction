using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;

[GlobalClass]
public partial class Area : Resource
{
    [Export]
    public Array<Vector2I> posRelative = new Array<Vector2I>();

}
