using Godot;
using System;

public partial class LabelAnimHandler<T> : ILabelAnimHandler where T : LabelAnimComponent, new()
{
	public override void addComponentsToLabels()
	{
		foreach (Label label in labels)
		{
			T t = new T();
			labelAnimComponents.Add(t);
			label.AddChild(t);
		}
	}
}
