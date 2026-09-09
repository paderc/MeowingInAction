using Godot;
using System;
using System.Threading.Tasks;

public partial class BaseAnim : Node3D
{
	[Export] protected bool waitForNext = true;
	AnimationPlayer player;
	[Signal]
	public delegate void FinishedEventHandler();
	public virtual void manageEntry(Entry entry)
	{

	}
	public override void _Ready()
	{
	}
	public virtual async Task play()
	{
		player = GetNode<AnimationPlayer>("%AnimationPlayer");
		if (player == null)
		{
			GD.PushError("AnimationPlayer is null");
			return;
		}

		if (!player.HasAnimation("Anim"))
		{
			GD.PushError("Animation 'Anim' not found");
			return;
		}
		player.Seek(0, true);
		player.Play("Anim");
		if (!waitForNext) return;
		double length = player.GetAnimation("Anim").Length;
		await ToSignal(GetTree().CreateTimer(length), SceneTreeTimer.SignalName.Timeout);
		player.Stop();
		EmitSignal(SignalName.Finished);
		this.QueueFree();
	}

}
