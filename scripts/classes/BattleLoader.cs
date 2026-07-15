using Godot;
using System;
using System.Linq;

public static class BattleLoader
{
	public static SerializableGrid getForStage(Stage stage)
	{
		DirAccess dir = DirAccess.Open(Paths.GridSavePath + "/" + stage.ToString());
		RandomNumberGenerator generator = new RandomNumberGenerator();
		int index = generator.RandiRange(0, dir.GetFiles().Length);
		return ResourceLoader.Load<SerializableGrid>(dir.GetFiles().ElementAt(index));
	}
	public static void loadOnto(SerializableGrid sGrid, BattleGrid battleContainer, Node3D battleSpace)
	{
		if (battleContainer != null)
		{
			battleContainer.QueueFree();
			battleContainer = null;
		}
		battleContainer = new BattleGrid(sGrid.sizeX, sGrid.sizeY, sGrid);
		battleSpace.AddChild(battleSpace);
	}
}
