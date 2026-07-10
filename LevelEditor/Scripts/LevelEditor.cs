using Godot;
using Godot.Collections;
using System;

public partial class LevelEditor : Control
{
	const string GridSavePath = "res://resources/Grids";

	LineEdit XParam;
	LineEdit YParam;
	Button Generate;
	Button Save;
	LineEdit MapName;
	Node3D GridSpace;
	BattleGrid battle;

	//Loading
	MenuButton LoadMenu;
	Dictionary<int, string> IndexToGridPath = new Dictionary<int, string>();

	//Painting
	OptionButton optionButton;
	GridType currentPaint;

	public override void _Ready()
	{
		getNodes();
		Generate.Pressed += () => generate();
		Save.Pressed += () => saveCurrentGrid();
		loadGrids();
		setupOptions();
	}

	public void getNodes()
	{
		// Adjust these paths to match your new scene tree:
		// UI (CanvasLayer) / VBoxContainer / ...
		XParam = GetNode<LineEdit>("UI/Parameters/XParam");
		YParam = GetNode<LineEdit>("UI/Parameters/YParam");
		Generate = GetNode<Button>("UI/Parameters/Generate");
		Save = GetNode<Button>("UI/Parameters/Save");
		MapName = GetNode<LineEdit>("UI/Parameters/MapName");
		optionButton = GetNode<OptionButton>("UI/Toolbar/Type/OptionButton");
		LoadMenu = GetNode<MenuButton>("UI/Parameters/Load");

		// GridSpace now lives in the 3D world branch, not under this Control
		GridSpace = GetNode<Node3D>("/root/LevelEditor/World/GridSpace");
	}

	public void loadSelectedGrid(long index)
	{
		if (!IndexToGridPath.TryGetValue((int)index, out string path)) return;

		if (battle != null)
		{
			battle.QueueFree();
			battle = null;
		}

		SerializableGrid sGrid = (SerializableGrid)ResourceLoader.Load(GridSavePath + "/" + path);
		battle = new BattleGrid(sGrid.sizeX, sGrid.sizeY, sGrid);
		GridSpace.AddChild(battle);
	}

	public void loadGrids()
	{
		DirAccess dir = DirAccess.Open(GridSavePath);
		int i = 0;
		PopupMenu popup = LoadMenu.GetPopup();
		popup.Clear();
		IndexToGridPath.Clear();
		foreach (string file in dir.GetFiles())
		{
			popup.AddCheckItem(file);
			IndexToGridPath.Add(i, file);
			i++;
		}
		popup.IdPressed += (id) => loadSelectedGrid(id);
	}

	public void saveCurrentGrid()
	{
		if (MapName.Text == null) return;
		if (battle == null) { GD.PrintErr("Cannot save empty battle"); return; }
		SerializableGrid serializableGrid = new SerializableGrid(MapName.Text, battle);
		ResourceSaver.Save(serializableGrid, GridSavePath + "/" + serializableGrid.name + ".res");
		loadGrids();
	}

	public void setupOptions()
	{
		if (GridBlock.typeToPathDict == null) GridBlock.findTexturePaths();
		foreach (GridType key in GridBlock.typeToPathDict.Keys)
		{
			optionButton.AddItem(key.ToString());
		}
		optionButton.ItemSelected += (index) => onOptionSelected(index);
	}

	public void onOptionSelected(long index)
	{
		string name = optionButton.GetItemText((int)index);
		if (!Enum.TryParse<GridType>(name, true, out GridType gridType)) GD.PrintErr("Didnt find a gridType of " + name);
		else currentPaint = gridType;
	}

	public void generate()
	{
		int x = 0;
		int y = 0;
		if (!int.TryParse(XParam.Text, out x) || !int.TryParse(YParam.Text, out y)) { GD.PushWarning("Cannot create such grid"); return; }
		if (x <= 0 || y <= 0) { GD.PushWarning("Cannot create such grid"); return; }
		if (battle != null)
		{
			battle.QueueFree();
		}
		battle = new BattleGrid(x, y);
		GridSpace.AddChild(battle);
	}

	private void changeCurrentBlock()
	{
		battle.currentHovered.setType(currentPaint);
	}

	bool paintedThisPress = false;

	public override void _Input(InputEvent @event)
	{
		if (battle == null) return;

		if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left)
		{
			if (mb.Pressed)
			{
				paintedThisPress = false;
				if (battle.currentHovered != null)
				{
					changeCurrentBlock();
					paintedThisPress = true;
				}
			}
		}
		// Optional: paint-while-dragging, block-by-block, without re-painting
		// the same block repeatedly on every mouse-motion event
		else if (@event is InputEventMouseMotion && Input.IsMouseButtonPressed(MouseButton.Left))
		{
			if (battle.currentHovered != null) changeCurrentBlock();
		}
	}
}
