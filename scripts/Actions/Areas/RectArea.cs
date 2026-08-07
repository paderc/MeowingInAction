using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class RectArea : Area
{
	private Vector2I _rectSize;

	[Export]
	public Vector2I rectSize
	{
		get => _rectSize;
		set
		{
			_rectSize = value;
			GeneratePosRelative();
		}
	}
	private void GeneratePosRelative()
	{
		posRelative = new Array<Vector2I>();

		int w = Math.Max(rectSize.X, 1);
		int h = Math.Max(rectSize.Y, 1);

		int startX = -Mathf.FloorToInt(w / 2f);
		int startY = -Mathf.FloorToInt(h / 2f);
		int endX = startX + w - 1;
		int endY = startY + h - 1;

		for (int x = startX; x <= endX; x++)
		{
			for (int y = startY; y <= endY; y++)
			{
				posRelative.Add(new Vector2I(x, y));
			}
		}
	}
}
