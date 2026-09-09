using Godot;
using System;

public partial class TypewriterLabelComp : LabelAnimComponent
{
	public override void _Ready()
	{
		base._Ready();
	}
	public override void _Process(double delta)
	{
		currentDelay -= delta;
		if (currentDelay < delta)
		{
			currentDelay = delayPerLetterS;
			currentIndex++;
			label.VisibleCharacters = currentIndex;
		}
	}
	public override void playAnimation()
	{
		base.playAnimation();
		label.VisibleCharacters = 0;
	}
}
