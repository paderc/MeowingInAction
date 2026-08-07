using Godot;
using System;

[GlobalClass]
public partial class Ally : Entity
{
    public override void setupEntityGUI(EntityGUI entityGUI)
    {
        MeshInstance3D meshInstance = entityGUI.GetNode<MeshInstance3D>("MeshInstance3D");
        StandardMaterial3D material = new StandardMaterial3D();
        material.AlbedoColor = Colors.Green;
        meshInstance.SetSurfaceOverrideMaterial(0, material);
    }
}
