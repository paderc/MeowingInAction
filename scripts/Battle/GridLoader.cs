using Godot;
using System;
using System.Linq;

public static class GridLoader
{
	public static BattleGrid getForStage(Stage stage)
	{
		RandomNumberGenerator generator = new RandomNumberGenerator();
		string basePath = Paths.GridSavePath + "/" + stage.ToString();
		DirAccess dir = DirAccess.Open(basePath);
		string[] files = dir.GetFiles();
		int index = generator.RandiRange(0, files.Length - 1);
		string fileName = files[index];
		string fullPath = basePath + "/" + fileName;
		SerializableGrid sGrid = ResourceLoader.Load<SerializableGrid>(fullPath);
		return new BattleGrid(sGrid);
	}
}
