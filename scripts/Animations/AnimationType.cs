using Godot;

[GlobalClass]
public partial class AnimationType : Resource
{
	public enum Category { TextAnim, Effect, Damage }
	public enum Instance { FlyTextAnim, BombAnim}
	[Export] public Category category;
	[Export] public Instance instance;
}
