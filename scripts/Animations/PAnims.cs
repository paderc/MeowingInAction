using Godot;
using Godot.Collections;
using System;
using System.Linq;

public static partial class PAnims
{

	public static float animationSpeed = 1;
	public static readonly Dictionary<AnimationType.Instance, PackedScene> animScenes = new()
	{
		{ AnimationType.Instance.FlyTextAnim, GD.Load<PackedScene>(Paths.flyTextAnimSceneUID)},
		{ AnimationType.Instance.BombAnim, GD.Load<PackedScene>(Paths.bombAnimSceneUID) }
	};
	public static BaseAnim GetAnim(Node3D parent, AnimationType.Instance animationInstanceType)
	{
		if (!animScenes.ContainsKey(animationInstanceType))
		{
			GD.PushError("No scene found for " + animationInstanceType.ToString());
			return null;
		}

		var scene = animScenes[animationInstanceType];
		if (!animationPlayable(parent)) return null;

		BaseAnim anim = scene.Instantiate<BaseAnim>();
		if (anim == null)
		{
			GD.PushError($"Failed to instantiate {typeof(BaseAnim).Name} from scene");
			return null;
		}

		parent.AddChild(anim);
		return anim;
	}
	static bool animationPlayable(Node node)
	{
		if (node == null) { GD.PushError("Cannot play animation on null"); return false; }
		if (!node.IsInsideTree()) { GD.PushError("Cannot play animation on node not inside tree"); return false; }
		return true;
	}
}
