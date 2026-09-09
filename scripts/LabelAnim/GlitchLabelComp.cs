using Godot;
using System;
using System.Linq;

public partial class GlitchLabelComp : LabelAnimComponent
{
	public int frameAmount = 2;
	RandomNumberGenerator generator = new RandomNumberGenerator();
	int frame = 0;
	public override void _Process(double delta)
	{
		currentDelay -= delta;
		
		if (frame < frameAmount)
		{
			frame++;
			
		}
		else
		{
			frame = 0;
			if (currentDelay > delta)
			{
				label.Text = makeGibberish();
			}
			else
			{
				currentDelay = delayPerLetterS;
				currentIndex++;
				if (currentIndex >= text.Length)
				{
					label.Text = text;
					SetProcess(false);
				}
			}
		}
	}
	
	string makeGibberish()
	{
		char[] gibberish = new char[text.Length];
		for (int i = 0; i < currentIndex; i++)
		{
			gibberish[i] = text[i];
		}
		for (int i = currentIndex; i < text.Length; i++)
		{
			gibberish[i] = randomLetter();
		}
		return new string(gibberish);
	}
	char randomLetter()
	{	
		return (char)generator.RandiRange(65, 126);
	}
	public override void playAnimation()
	{
		base.playAnimation();
		label.VisibleCharacters = -1;
		GD.Print(text);
	}
}
