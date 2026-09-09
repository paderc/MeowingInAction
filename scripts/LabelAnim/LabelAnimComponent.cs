using Godot;

public abstract partial class LabelAnimComponent : Node
{
	protected double delayPerWordS = 2;
	protected double delayPerLetterS;
	protected double currentDelay;
	protected Label label;
	protected string text;
	protected int currentIndex = 0;
	public override void _Ready()
	{
		SetProcess(false);
		label = GetParent<Label>();
		text = label.Text;
		delayPerLetterS = delayPerWordS / text.Length;
		label.VisibleCharacters = 0;
	}
	public virtual void playAnimation()
	{
		label.VisibleCharacters = -1;
		currentIndex = 0;
		currentDelay = delayPerLetterS;
		SetProcess(true);
	}
}
