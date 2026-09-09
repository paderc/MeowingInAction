using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

public partial class ButtonReactNode : Node
{
	[Export]
	BoxContainer buttonContainer;
	[Export]
	float hoverScale = 1.3f;
	[Export]
	float separatorScale = 1.3f;
	[Export]
	float resizeDuration = 0.1f;
	List<Control> controls = new();
	public override void _Ready()
	{
		addButtons();
	}
	void addButtons()
	{
		if (buttonContainer == null)
		{
			Logger.IncludeStackTraces = true;
			Logger.Warning("Button container not assigned");
			return;
		}
		foreach (Node node in buttonContainer.GetChildren())
		{
			if (!(node is Control ctrl)) { continue; }
			controls.Add(ctrl);
			if (node is BaseButton button)
			{
				button.OffsetTransformEnabled = true;
				button.Resized += () => button.PivotOffset = button.Size / 2f;
				button.MouseEntered += () => onButtonHovered(button);
				button.MouseExited += () => onButtonUnhovered(button);
			}
			else if (node is Separator separator)
			{
				separator.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
				separator.Modulate = Colors.Transparent;
			}
		}
	}
	void onButtonHovered(BaseButton button)
	{
		changeScale(button, hoverScale);
		makeNeighboursBigger(button);
	}
	void makeNeighboursBigger(BaseButton button)
	{
		int buttonIndex = controls.IndexOf(button);
		Separator firstSeparator = controls.ElementAtOrDefault(buttonIndex - 1) as Separator;
		if (firstSeparator != null)
		{

			changeScale(firstSeparator, separatorScale);
		}
		Separator secondSeparator = controls.ElementAtOrDefault(buttonIndex + 1) as Separator;
		if (secondSeparator != null)
		{
			secondSeparator.SizeFlagsStretchRatio = separatorScale;
		}
	}
	void onButtonUnhovered(BaseButton button)
	{
		resetScale(button);
		resetNeighbours(button);
	}
	void resetNeighbours(BaseButton button)
	{
		int buttonIndex = controls.IndexOf(button);
		Separator firstSeparator = controls.ElementAtOrDefault(buttonIndex - 1) as Separator;
		if (firstSeparator != null)
		{
			resetScale(firstSeparator);
		}
		Separator secondSeparator = controls.ElementAtOrDefault(buttonIndex + 1) as Separator;
		if (secondSeparator != null)
		{
			resetScale(secondSeparator);
		}
	}
	void changeScale(Control control, float scale)
	{
		Tween tween = CreateTween();
		tween.TweenProperty(control, "offset_transform_scale", Vector2.One * scale, resizeDuration);
	}
	void resetScale(Control control)
	{
		Tween tween = CreateTween();
		tween.TweenProperty(control, "offset_transform_scale", Vector2.One, resizeDuration);
	}

}
