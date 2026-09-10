using Godot;
using System;

public partial class TextAnim : BaseAnim
{
	public override void manageEntry(Entry entry)
	{
		text = entry.amount.ToString();
		Scale *= entry.amount;
	}
	public string text
	{
		set
		{
			if (value == null) { return; }
			if (textMesh == null) { GD.PushError("TextMesh is null"); return; }
			if (Int32.TryParse(value, out int number)) { double fontSize = textMesh.FontSize * 0.7 * number; textMesh.FontSize = (int)fontSize; }
			textMesh.Text = value;
		}
	}
	TextMesh textMesh;
	public override void _Ready()
	{
		GpuParticles3D textParticles = GetNode<GpuParticles3D>("%TextParticles");
		TextMesh original = textParticles.DrawPass1 as TextMesh;
		if (original != null)
		{
			textMesh = new TextMesh
			{
				Font = original.Font,
				FontSize = original.FontSize,
				Material = original.Material
			};
			textParticles.DrawPass1 = textMesh;
			text = "JEB";
		}
	}
}
