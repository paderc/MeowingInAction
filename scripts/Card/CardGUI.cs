using Godot;
using System;

public partial class CardGUI : Control
{
	static string cardGUIPath = "res://scenes/CardGUI.tscn";

	public Draggable draggable;
	Card card;
	Label costLabel;
	
	public CardGUI()
	{
		draggable = new Draggable(this);
	}
	public override void _Ready()
	{
		costLabel = GetNode<Label>("MarginContainer/Control/CostBG/Cost");
		updateUI();
	}
	void updateUI()
	{
		costLabel.Text = card.cost.ToString();
	}
	public static CardGUI GetCardGUI(Card card)
	{
		PackedScene scene = GD.Load<PackedScene>(cardGUIPath);
		CardGUI cardGUI = scene.Instantiate<CardGUI>();

		cardGUI.card = card;

		return cardGUI;
	}
}
