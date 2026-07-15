using Godot;
using System;

public partial class NameButton : Button
{
	public override void _Ready()
	{
		this.Text = this.Name;
		this.SizeFlagsVertical = SizeFlags.ExpandFill;
		this.SizeFlagsHorizontal = SizeFlags.ExpandFill;
	}

}
