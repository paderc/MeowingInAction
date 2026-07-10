using Godot;
using Godot.Collections;
using System;
using System.Linq;



public partial class BattleGrid : Control
{
	const string basicTexturePath = "res://resources/images/backgroundNoiseTexture.res";
	float blockSize;
	bool mouseOnGrid;
	TextureRect hoverOutline;
	Resource basicTexture = GD.Load(basicTexturePath);
	public GridBlock currentHovered;
	public Vector2I gridSize;
	public Array<GridBlock> blocks = new Array<GridBlock>();
	Control gridContainer = new Control();

	private Array<SerializableBlock> serializableBlocks = null;

	public BattleGrid(int x, int y)
	{
		gridSize = new Vector2I(x,y);
	}

	public BattleGrid(int x, int y, SerializableGrid sGrid)
	{
		gridSize = new Vector2I(x, y);
		serializableBlocks = sGrid.grid;
	}
	public override void _Ready()
	{
		this.SetAnchorsPreset(LayoutPreset.FullRect);
		this.SetOffsetsPreset(LayoutPreset.FullRect);
		initializeGrid();
	}

	public void initializeGrid()
	{
		setupGridContainer(calculateGridPixelSize());

		if (serializableBlocks == null) setupBlocks();
		else setupBlocks(serializableBlocks);
		
		setupHover("res://resources/Images/battleGridHoverOutline.png");
	}

	Vector2 calculateGridPixelSize()
	{
		Vector2 available_size = new Vector2(
			this.Size.X * 0.8f,
			this.Size.Y * 0.8f
			);

		if (available_size.X <= 0 || available_size.Y <= 0)
		{
			available_size = new Vector2(600, 400);
		}

		float maxXSize = available_size.X / gridSize.X;
		float maxYSize = available_size.Y / gridSize.Y;

		blockSize = Math.Min(maxXSize, maxYSize);

		return new Vector2(
			gridSize.X * blockSize,
			gridSize.Y * blockSize
			);

	}

	void setupGridContainer(Vector2 gridPixelSize)
	{
		gridContainer.Position = new Vector2(
			(Size.X - gridPixelSize.X) / 2f,
			(Size.Y - gridPixelSize.Y) / 2f
			);
		gridContainer.MouseExited += () =>
		{
			if (!gridContainer.GetGlobalRect().HasPoint(GetGlobalMousePosition())) hoverOutline.Visible = false;
		};
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
				
				block.Position = new Vector2(
					blockSize * x,
					blockSize * (gridSize.Y - y - 1)
					);
				block.MouseEntered += () =>
				{
					onBlockHovered(block);
				};
				block.MouseExited += () => 
				{
					onBlockUnhovered(block);
				};
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
					serializableBlocks.ElementAt(x * gridSize.X + y ).type,
					new Vector2I(x, y),
					blockSize
					);

				blocks.Add(block);

				block.Position = new Vector2(
					blockSize * x,
					blockSize * (gridSize.Y - y - 1)
					);
				block.MouseEntered += () =>
				{
					onBlockHovered(block);
				};
				block.MouseExited += () =>
				{
					onBlockUnhovered(block);
				};
				gridContainer.AddChild(block);
			}
		}
	}

	void setupHover(String outlinePath)
	{
		hoverOutline = new TextureRect();
		hoverOutline.MouseFilter = MouseFilterEnum.Ignore;
		hoverOutline.Size = new Vector2(
			blockSize,
			blockSize
			);
		hoverOutline.StretchMode = TextureRect.StretchModeEnum.Scale;
		hoverOutline.Texture = (Texture2D)GD.Load(outlinePath);
		hoverOutline.Visible = false;
		this.AddChild(hoverOutline);
	}

	public void onBlockHovered(GridBlock block)
	{
		currentHovered = block;
		hoverOutline.Visible = true;
		hoverOutline.GlobalPosition = block.GlobalPosition;
	}
	public void onBlockUnhovered(GridBlock block)
	{
		currentHovered = null;
	}
}
