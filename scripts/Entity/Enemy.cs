using Godot;
using System;

public partial class Enemy : Entity
{
	public override void setupEntityGUI(EntityGUI entityGUI)
	{
		MeshInstance3D meshInstance = entityGUI.GetNode<MeshInstance3D>("MeshInstance3D");
		StandardMaterial3D material = new StandardMaterial3D();
		material.AlbedoColor = Colors.Red;
		meshInstance.SetSurfaceOverrideMaterial(0, material);
	}
}
