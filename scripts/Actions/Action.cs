using Godot;
using System;

[GlobalClass]
public abstract partial class Action : Resource
{
	public abstract void perform(CardActionHandler cardActionHandler);
}
