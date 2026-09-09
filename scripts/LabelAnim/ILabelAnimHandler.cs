using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public abstract partial class ILabelAnimHandler : Node
{
	protected Array<Label> labels = new Array<Label>();
	protected List<LabelAnimComponent> labelAnimComponents = new List<LabelAnimComponent>();
	public void addLabel(Label label)
	{
		if (label == null || label.GetType() != typeof(Label)) GD.PrintErr($"Some path label incorrect");
		labels.Add(label);
	}
	public abstract void addComponentsToLabels();
	public void playAnimations()
	{
		foreach(LabelAnimComponent labelAnimComponent in labelAnimComponents)
		{
			labelAnimComponent.playAnimation();
		}
	}
}
