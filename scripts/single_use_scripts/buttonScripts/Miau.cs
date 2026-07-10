using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public partial class Miau : NameButton
{
	List<AudioStream> soundFiles = new List<AudioStream>();
	AudioStreamPlayer soundPlayer = new AudioStreamPlayer();
	List<TextureRect> imageList = new List<TextureRect>();
	public override void _Ready()
	{
		Pressed += onButtonPressed;
		AddChild(soundPlayer);
		loadSoundFiles();
		findCatImages();
	}

	public async void onButtonPressed()
	{
		ButtonPressed = true;
		Disabled = true;
		spinCatImages();
		playRandomMeow();
		await ToSignal(soundPlayer, AudioStreamPlayer.SignalName.Finished);
		Disabled = false;
		ButtonPressed = false;
	}
	public void spinCatImages()
	{
		foreach (TextureRect cat in imageList)
		{
			switch ((cat.FlipH, cat.FlipV))
			{
				case (false, false):
					cat.FlipH = true;
					break;
				case (true, false):
					cat.FlipV = true;
					break;
				case (true, true):
					cat.FlipH = false;
					break;
				case (false, true):
					cat.FlipV = false;
					break;
			}
		}
	}

	private void playRandomMeow()
	{
		if (soundFiles == null) GD.PushError("Found no cat noises");
		RandomNumberGenerator generator = new RandomNumberGenerator();
		int randomIndex = generator.RandiRange(0, soundFiles.Count - 1);
		soundPlayer.Stream = soundFiles[randomIndex];
		soundPlayer.Play();
	}

	private void loadSoundFiles()
	{
		string soundPath = "res://resources/sound/meowSounds";

		using DirAccess dir = DirAccess.Open(soundPath);
		if (dir == null) { GD.PushError($"Failed to open directory: {soundPath}"); return;}
		string[] files = dir.GetFiles();

		if (files.Length == 0) { GD.PushWarning($"No files found in {soundPath}"); return; }

		

		foreach (string file in files)
		{
			string fullPath = soundPath + "/" + file;
			if (file.EndsWith(".import")) continue;
			if (file.ToLower().EndsWith(".mp3"))
			{
				AudioStreamMP3 mp3Stream = GD.Load<AudioStreamMP3>(fullPath);
				if (mp3Stream != null)
				{
					soundFiles.Add(mp3Stream);
				}
			}
		}
	}

	private void findCatImages()
	{
		foreach (Node child in GetParent().GetParent().GetParent().FindChild("Cats").GetChildren())
		{
			imageList.Add((TextureRect)child);
		}
	}
}
