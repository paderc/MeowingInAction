using Godot;
using System;

public partial class AmountLabel : Label
{
	int _amount;
	public int amount
	{
		get { return _amount; }
		set
		{ 
			if (value.GetType() != typeof(int)) { GD.PushWarning("Setting non-int amount of amountLabel"); }
			this.Text = value.ToString();
			_amount = value;
		}
	}
	public override void _Ready()
	{
		amount = 0;
	}
}
