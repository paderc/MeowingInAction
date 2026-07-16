using Godot;
using System;
using System.Linq;

public static class GridLoader
{
	public static BattleGrid getForStage(Stage stage)
	{
		DirAccess dir = DirAccess.Open(Paths.GridSavePath + "/" + stage.ToString());
		RandomNumberGenerator generator = new RandomNumberGenerator();
		int index = generator.RandiRange(0, dir.GetFiles().Length);
		SerializableGrid sGrid = ResourceLoader.Load<SerializableGrid>(dir.GetFiles().ElementAt(index));
        return new BattleGrid(sGrid);
	}
	public static void loadOnto(Stage stage, BattleGrid battleContainer, Node3D battleSpace)
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
