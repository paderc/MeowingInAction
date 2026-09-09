using Castle.Core.Configuration;
using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public abstract partial class BlockAction : CardAction
{
	public async override sealed Task confirmChange(CardActionHandler cardActionHandler)
	{
		Array<GridBlock> blocks = cardActionHandler.currentPlayed.Item2;
		foreach (GridBlock block in blocks)
		{
			await confirmChangesLikeOn(block);
		}
	}
	public abstract Task confirmChangesLikeOn(GridBlock block);

	public override sealed void preview(CardActionHandler cardActionHandler)
	{
		Array<GridBlock> blocks = new(cardActionHandler.hoveredBlocks);
		foreach (GridBlock block in blocks)
		{
			previewLikeOn(block);
		}
	}
	public abstract void previewLikeOn(GridBlock block);

	public override sealed void undoPreview(CardActionHandler cardActionHandler)
	{
		Array<GridBlock> blocks = new (cardActionHandler.previewBlocks);
		foreach (GridBlock block in blocks)
		{
			undoPreviewLikeOn(block);
		}
	}
	public abstract void undoPreviewLikeOn(GridBlock block);
}
