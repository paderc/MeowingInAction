using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[GlobalClass]
public partial class DamageAction : BlockAction
{
	[Export] DamageEntry entry;
	[Export] Array<AnimationType> animations;
	List<Node> animPlayers;
	public override async Task confirmChangesLikeOn(GridBlock block)
	{
		for (int i = 0; i < animations.Count; i++)
		{
			AnimationType animationType = animations[i];
			BaseAnim anim = PAnims.GetAnim(block, animationType.instance);
			anim.manageEntry(entry);
			await anim.play();
			if (animationType.category == AnimationType.Category.Damage)
			{
				block.entityHandler.makeDamagePermanent(entry.amount);
				GD.Print(entry.ToString() + " to " + block.ToString());
			}
		}
	}

	public override void previewLikeOn(GridBlock block)
	{
		block.entityHandler.previewDamage(entry.amount);
	}

	public override void undoPreviewLikeOn(GridBlock block)
	{
		block.entityHandler.undoPreviewDamage();
	}
}
