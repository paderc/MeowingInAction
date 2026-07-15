using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BattleGrid : Node3D
{
	float blockSize = 2.0f;
	public GridBlock currentHovered;
	public Vector2I gridSize;
	public Array<GridBlock> blocks = new Array<GridBlock>();
	Node3D gridContainer;

	private Array<SerializableBlock> serializableBlocks = null;

	public BattleGrid(int x, int y)
	{
		gridSize = new Vector2I(x, y);
		gridContainer = new Node3D();

	}

	public BattleGrid(int x, int y, SerializableGrid sGrid)
	{
		gridSize = new Vector2I(x, y);
		serializableBlocks = sGrid.grid;
		gridContainer = new Node3D();

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
			for (int y = 0; y < gridSize.Y; y++)
			{
				GridBlock block = new GridBlock(
					GridType.NULL,
					new Vector2I(x, y),
					blockSize
					);

				blocks.Add(block);

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

	public void setupBlocks(Array<SerializableBlock> serializableBlocks)
	{
		for (int x = 0; x < gridSize.X; x++)
		{
			for (int y = 0; y < gridSize.Y; y++)
			{
				GridBlock block = new GridBlock(
					serializableBlocks.ElementAt(x * gridSize.X + y).type,
					new Vector2I(x, y),
					blockSize
					);

				blocks.Add(block);

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

	public void onBlockHovered(GridBlock block)
	{
		currentHovered = block;
		block.setHovered(true);
	}

	public void onBlockUnhovered(GridBlock block)
	{
		if (currentHovered == block) currentHovered = null;
		block.setHovered(false);
	}
}
