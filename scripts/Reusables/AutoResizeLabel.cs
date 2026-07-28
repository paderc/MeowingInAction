using Godot;
using System;

[Tool]
public partial class AutoResizeLabel : Node
{
	[Export(PropertyHint.Range, "0,1,")]
	public float ParentOccupationFraction;
	public override void _Ready()
	{
		Label label = GetParent<Label>();
		label.Resized += resize;
	}

	void resize()
	{
		Label label = GetParent<Label>();
		Control parent = label.GetParent<Control>();
		label.LabelSettings.FontSize = (int)(parent.Size.Y * ParentOccupationFraction);
	}
}
