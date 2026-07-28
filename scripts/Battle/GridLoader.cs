using Godot;
using System;
using System.Linq;

public static class GridLoader
{
	public static Battle getForStage(Stage stage)
	{
		RandomNumberGenerator generator = new RandomNumberGenerator();
		string basePath = Paths.GridSavePath + "/" + stage.ToString();
		DirAccess dir = DirAccess.Open(basePath);
		string[] files = dir.GetFiles();
		int index = generator.RandiRange(0, files.Length - 1);
		string fileName = files[index];
		string fullPath = basePath + "/" + fileName;
		SerializableGrid sGrid = ResourceLoader.Load<SerializableGrid>(fullPath);
		return new Battle(sGrid);
	}
	public static void loadOnto(Stage stage, Battle battleContainer, Node3D battleSpace)
	{
		if (battleContainer != null)
		{
			battleContainer.QueueFree();
			battleContainer = null;
		}
		battleContainer = getForStage(stage);
		battleSpace.AddChild(battleSpace);
	}
}
