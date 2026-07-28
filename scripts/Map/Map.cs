using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Map : TextureRect
{
    static string mapPath = "res://scenes/map.tscn";

    int levelCount;
	public Stage stage;
	public Array<MapChoice> choices = new Array<MapChoice>();
	public Map(Stage stage)
	{
		this.Name = stage.ToString() + " map";
		this.stage = stage;
		setupBackground();
	}

	public override void _Ready()
	{
		CallDeferred(nameof(setupLevels));

	}
	public static Map create()
	{
        PackedScene scene = GD.Load<PackedScene>(mapPath);
        Map map = scene.Instantiate<Map>();
        if (map == null) GD.PushError("Did not find ingame menu scene at " + mapPath);
        return map;
    }
	private void setupLevels()
	{
		float margin = 50f;
		float mapWidth = Size.X;
		float mapHeight = Size.Y;

		//Start
		var startChoice = new MapChoice();
		startChoice.Position = new Vector2(margin, mapHeight / 2f);
		AddChild(startChoice);
		choices.Add(startChoice);

		//Middle
		int pathCount = 3;

		float choiceWidth = startChoice.Size.X;

		float middleWidth = mapWidth - 2 * (margin + choiceWidth);

		float xStep = middleWidth / (levelCount + 1);
		float yOffset = mapHeight / (pathCount + 1);

		for (int path = 0; path < pathCount; path++)
		{
			float yPos = yOffset * (path + 1);
			for (int level = 0; level < levelCount; level++)
			{
				var choice = new MapChoice();
				float xPos = margin + choiceWidth + (level + 1) * xStep;
				choice.Position = new Vector2(xPos, yPos);
				AddChild(choice);
				choices.Add(choice);
			}
		}

		//End
		var endChoice = new MapChoice();
		endChoice.Position = new Vector2(mapWidth - margin - choiceWidth, mapHeight / 2f);
		AddChild(endChoice);
		choices.Add(endChoice);
	}

	void setupBackground()
	{
		this.SetAnchorsPreset(LayoutPreset.FullRect);

		string fullPath = Paths.mapBackground + "/" + stage.ToString() + ".tres";

		MapRes mapRes = GD.Load<MapRes>(fullPath);
		if (mapRes == null) GD.PushError("Map image not found at: " + fullPath);
		this.Texture = mapRes.texture;
		this.levelCount = mapRes.levelCount;
	}
}
