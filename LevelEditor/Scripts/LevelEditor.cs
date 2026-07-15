using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class LevelEditor : Control
{
	LineEdit XParam;
	LineEdit YParam;
	Button Generate;
	Button Save;
	
	Node3D GridSpace;
	BattleGrid battle;

	//Map parameters
	LineEdit MapName;
	OptionButton mapStage;
	Stage currentStage;
	Dictionary<int, Stage> IndexToStage = new Dictionary<int, Stage>();

	//Loading
	MenuButton LoadMenu;
	Dictionary<int, string> IndexToGridPath = new Dictionary<int, string>();

	//Painting
	OptionButton blockBrushOption;
	GridType currentPaint;

	public override void _Ready()
	{
		getNodes();
		Generate.Pressed += () => generate();
		Save.Pressed += () => saveCurrentGrid();
		loadGrids();
		LoadMenu.GetPopup().IdPressed += (id) => loadSelectedGrid(id);
		setupOptions();
		setupMapStages();
	}

	public void getNodes()
	{
		XParam = GetNode<LineEdit>("UI/Parameters/XParam");
		YParam = GetNode<LineEdit>("UI/Parameters/YParam");
		Generate = GetNode<Button>("UI/Parameters/Generate");
		Save = GetNode<Button>("UI/Parameters/Save");
		MapName = GetNode<LineEdit>("UI/Parameters/MapParameters/MapName");
		mapStage = GetNode<OptionButton>("UI/Parameters/MapParameters/MapStage");
		blockBrushOption = GetNode<OptionButton>("UI/Toolbar/Type/OptionButton");
		LoadMenu = GetNode<MenuButton>("UI/Parameters/Load");
		GridSpace = GetNode<Node3D>("Battle/GridSpace");
	}

	void setupMapStages()
	{
		PopupMenu popupMenu = mapStage.GetPopup();
		int i = 0;
		foreach (Stage stage in Enum.GetValues(typeof(Stage)))
		{
			IndexToStage.Add(i, stage);
			popupMenu.AddItem(stage.ToString());
			i++;
		}
		popupMenu.IdPressed += (id) => changeCurrentStage(id);
	}

	void changeCurrentStage(long index)
	{
		IndexToStage.TryGetValue((int)index, out Stage stage);
		currentStage = stage;
		loadGrids();
	}

	public void loadSelectedGrid(long index)
	{
		if (!IndexToGridPath.TryGetValue((int)index, out string path)) return;
		SerializableGrid sGrid = (SerializableGrid)ResourceLoader.Load(Paths.GridSavePath + "/" + path);
		BattleLoader.loadOnto(sGrid, battle, GridSpace);
	}

	public void loadGrids()
	{
		DirAccess dir = DirAccess.Open(Paths.GridSavePath + "/" + currentStage.ToString());
		if (dir == null)
		{
			GD.PrintErr($"Cannot open directory: {Paths.GridSavePath}");
			return;
		}
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
	}

	public void saveCurrentGrid()
	{
		if (string.IsNullOrEmpty(MapName.Text))
		{
			GD.PrintErr("Map name is empty.");
			return;
		}
		if (battle == null)
		{
			GD.PrintErr("Cannot save empty battle.");
			return;
		}

		string godotDirPath = Paths.GridSavePath + "/" + currentStage.ToString();
		string osDirPath = ProjectSettings.GlobalizePath(godotDirPath);

		Directory.CreateDirectory(osDirPath);

		SerializableGrid serializableGrid = new SerializableGrid(MapName.Text, battle);
		string filePath = godotDirPath + "/" + serializableGrid.name + ".res";
		ResourceSaver.Save(serializableGrid, filePath);
		loadGrids();

	}

	public void setupOptions()
	{
		if (GridBlock.typeToPathDict == null) GridBlock.findTexturePaths();
		foreach (GridType key in GridBlock.typeToPathDict.Keys)
		{
			blockBrushOption.AddItem(key.ToString());
		}
		blockBrushOption.ItemSelected += (index) => onOptionSelected(index);
	}

	public void onOptionSelected(long index)
	{
		string name = blockBrushOption.GetItemText((int)index);
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
		else if (@event is InputEventMouseMotion && Input.IsMouseButtonPressed(MouseButton.Left))
		{
			if (battle.currentHovered != null) changeCurrentBlock();
		}
	}
}
