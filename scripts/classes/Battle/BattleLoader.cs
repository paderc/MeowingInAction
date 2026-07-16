using Godot;
using System;

public partial class BattleLoader
{
	public BattleGrid battleGrid;
	PackedScene battleScene = GD.Load<PackedScene>(Paths.battleScenePath);
	public BattleLoader(Stage stage)
	{
		battleGrid = GridLoader.getForStage(stage);
	}

	public BattleLoader(BattleGrid battleGrid)
	{
		this.battleGrid = battleGrid;
	}

	public Node addBattleTo(Control node)
	{
		Node scene = battleScene.Instantiate();
		Node3D gridSpace = scene.GetNode<Node3D>("GridSpace");
		gridSpace.AddChild(battleGrid);

		node.AddChild(scene);
		return scene;
	}
}
