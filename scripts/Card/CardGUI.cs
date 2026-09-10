using Godot;
using System;
using System.Threading.Tasks;

public partial class CardGUI : Control
{
	static string cardGUIPath = "res://scenes/CardGUI.tscn";

	public bool justPlayed = false;
	public Draggable draggable;
	public Card card;
	Label costLabel, nameLabel, descLabel;
	Tween currentTween;
	public CardGUI()
	{
		draggable = new Draggable(this);
	}
	public override void _Ready()
	{
		costLabel = GetNode<Label>("%CostLabel");
		nameLabel = GetNode<Label>("%TitleLabel");
		descLabel = GetNode<Label>("%DescLabel");
		updateUI();
	}
	void updateUI()
	{
		costLabel.Text = card.cost.ToString();
		nameLabel.Text = card.name;
		descLabel.Text = card.description;
	}
	public async Task moveToGlobal(Vector2 globalTarget, bool useFx = false)
	{
		const float DURATION = 0.5f;
        if (currentTween != null && currentTween.IsRunning())
            currentTween.Kill();

        currentTween = CreateTween();
        currentTween.TweenProperty(this, "global_position", globalTarget, DURATION)
                    .SetEase(Tween.EaseType.Out);
        await ToSignal(currentTween, Tween.SignalName.Finished);
        currentTween = null;
    }
    public async Task moveToLocal(Vector2 localTarget, bool useFx = false)
    {
        const float DURATION = 0.5f;
        if (currentTween != null && currentTween.IsRunning())
            currentTween.Kill();

        currentTween = CreateTween();
        currentTween.TweenProperty(this, "position", localTarget, DURATION)
                    .SetEase(Tween.EaseType.Out);
        await ToSignal(currentTween, Tween.SignalName.Finished);
        currentTween = null;
    }
    public void makeTransparent()
	{
		Color color = this.Modulate;
		color.A = 0.8f;
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
