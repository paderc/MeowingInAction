using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BattleGrid : Node3D
{
	[Signal]
	public delegate void HoverUpdatedEventHandler(GridBlock block);

	float blockSize = 2.0f;

	public HashSet<Vector2I> hoverSet = new HashSet<Vector2I>();
	public GridBlock currentHovered;
	public Array<GridBlock> hoverExtra = new Array<GridBlock>();
	
	Node3D gridContainer = new Node3D();

	public Vector2I gridSize;
	public Array<Array<GridBlock>> blocks = new Array<Array<GridBlock>>();
	private Array<Array<SerializableBlock>> serializableBlocks = null;
	private BattleGrid()
	{

	}
	public BattleGrid(int x, int y) : this()
	{
		gridSize = new Vector2I(x, y);
	}

	public BattleGrid(SerializableGrid sGrid) : this()
	{
		gridSize = new Vector2I(sGrid.sizeX, sGrid.sizeY);
		serializableBlocks = sGrid.grid;
	}
	public void addEntity(Entity entity)
	{
		
	}
	public override void _Ready()
	{
		initializeGrid();
		this.Name = "BattleGrid";
		gridContainer.Name = "GridContainer";
	}
	public void initializeGrid()
	{
		setupGridContainer();

		if (serializableBlocks == null) setupBlocks();
		else setupBlocks(serializableBlocks);
	}

	void setupGridContainer()
	{
		gridContainer.Position = new Vector3(
			-(gridSize.X * blockSize) / 2,
			0,
			-(gridSize.Y * blockSize) / 2
			);
		AddChild(gridContainer);
	}

	void setupBlocks()
	{
		for (int x = 0; x < gridSize.X; x++)
		{
			Array<GridBlock> row = new Array<GridBlock>();
			blocks.Add(row);
			for (int y = 0; y < gridSize.Y; y++)
			{
				GridBlock block = new GridBlock(
					GridType.NULL,
					new Vector2I(x, y),
					blockSize
					);

				row.Add(block);

				block.Position = new Vector3(
					blockSize * x,
					0,
					blockSize * y
					);

				block.MouseEntered += () => onBlockHovered(block);
				block.MouseExited += () => onBlockUnhovered(block);

				gridContainer.AddChild(block);
			}
		}
	}

	public void setupBlocks(Array<Array<SerializableBlock>> serializableBlocks)
	{
		for (int x = 0; x < gridSize.X; x++)
		{
			Array<GridBlock> column = new Array<GridBlock>();
			blocks.Add(column);
			for (int y = 0; y < gridSize.Y; y++)
			{
				GridBlock block = new GridBlock(
					serializableBlocks.ElementAt(x).ElementAt(y).type,
					new Vector2I(x, y),
					blockSize
					);

				column.Add(block);

				block.Position = new Vector3(
					blockSize * x,
					0,
					blockSize * y
					);

				block.MouseEntered += () => 
				{
					onBlockHovered(block);
					EmitSignal(SignalName.HoverUpdated, block);
				};
				block.MouseExited += () =>
				{
					onBlockUnhovered(block);
					EmitSignal(SignalName.HoverUpdated, null);
				};
				gridContainer.AddChild(block);
			}
		}
	}

	public void onBlockHovered(GridBlock block)
	{
		foreach (Vector2I vector in hoverSet)
		{
			foreach (Array<GridBlock> row in blocks)
			{
				IEnumerable<GridBlock> hoverBlocks = row.Where(gridBlock => gridBlock.gridPosition == block.gridPosition + vector);
				foreach (GridBlock gridBlock in hoverBlocks)
				{
					hoverExtra.Add(gridBlock);
					gridBlock.setHovered(true);
				}
			}
		}
		currentHovered = block;
		block.setHovered(true);
	}

	public void onBlockUnhovered(GridBlock block)
	{
		if (currentHovered == block) currentHovered = null;
		foreach(GridBlock gridBlock in hoverExtra)
		{
			gridBlock.setHovered(false);
		}
		block.setHovered(false);
	}
	public Node3D getBattleGridNode()
	{
		PackedScene battleScene = GD.Load<PackedScene>(Paths.battleGridScene);
		Node3D scene = battleScene.Instantiate<Node3D>();
		Node3D gridSpace = scene.GetNode<Node3D>("GridSpace");
		gridSpace.AddChild(this);
		return scene;
	}
	public static BattleGrid getBattleGrid(Stage stage)
	{
		BattleGrid battleGrid = GridLoader.getForStage(stage);
		if (battleGrid == null) { GD.PushError("Battle grid is null, also returning null"); return null; }
		return battleGrid;
	}
	public void changeHoverArea(Area area)
	{
		hoverSet = new HashSet<Vector2I>();

		foreach (Vector2I vector in area.posRelative)
		{
			hoverSet.Add(vector);
		}
	}
	public void resetHoverArea()
	{
		hoverSet = null;
	}
}
