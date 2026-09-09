using Godot;
using System;
using System.Collections.Generic;

public static class ShaderWarmup
{
	public static void WarmUpAnimations(Node tempParent)
	{
		foreach (PackedScene scene in PAnims.animScenes.Values)
		{
			Node anim = scene.Instantiate();
			tempParent.AddChild(anim);
			anim.QueueFree();
		}
	}
	public static void WarmUpEntityGUI(Node tempParent)
	{
		PackedScene scene = GD.Load<PackedScene>(Paths.entityGUIUID);
		EntityGUI entityGUI = scene.Instantiate<EntityGUI>();
		tempParent.AddChild(entityGUI);
		entityGUI.QueueFree();
	}
}
