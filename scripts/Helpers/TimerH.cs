using Godot;
using System;

public static partial class TimerH
{
	public static SignalAwaiter cooldown(Node parent, float time)
	{
		Timer timer = new Timer();
		parent.AddChild(timer);
		timer.OneShot = true;
		timer.Start(1);
		timer.Timeout += () => timer.QueueFree();
		return parent.ToSignal(timer, Timer.SignalName.Timeout);
	}
}
