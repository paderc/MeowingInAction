using Godot;
using System;

public partial class CardGUI : Control
{
	static string cardGUIPath = "res://scenes/CardGUI.tscn";

	public Draggable draggable;
	public Card card;
	Label costLabel;
	Label nameLabel;
	Label descLabel;
	
	public CardGUI()
	{
		draggable = new Draggable(this);
	}
	public override void _Ready()
	{
		costLabel = GetNode<Label>("MarginContainer/Control/CostBG/Cost");
		nameLabel = GetNode<Label>("MarginContainer/Control/TitleBG/TitleLabel");
		descLabel = GetNode<Label>("MarginContainer/Control/TitleBG/TitleLabel");
		updateUI();
	}
	void updateUI()
	{
		costLabel.Text = card.cost.ToString();
		nameLabel.Text = card.name;
		descLabel.Text = card.description;
	}
	public void makeTransparent()
	{
		Color color = this.Modulate;
		color.A = 0.5f;
		this.Modulate = color;
	}
	public void restoreTransparency()
	{
		Color color = this.Modulate;
		color.A = 1f;
		this.Modulate = color;
	}
	public static CardGUI GetCardGUI(Card card)
	{
		PackedScene scene = GD.Load<PackedScene>(cardGUIPath);
		CardGUI cardGUI = scene.Instantiate<CardGUI>();

		cardGUI.card = card;

		return cardGUI;
	}
}
