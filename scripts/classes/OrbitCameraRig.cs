using Godot;

public partial class OrbitCameraRig : Node3D
{
	[Export] float rotateSpeed = 0.01f;
	[Export] float zoomSpeed = 0.5f;
	[Export] float minZoom = 3f;
	[Export] float maxZoom = 30f;
	[Export] float minPitch = -80f;
	[Export] float maxPitch = -5f;

	Node3D pitchNode;
	Camera3D camera;
	bool dragging;
	float currentZoom = 10f;

	public override void _Ready()
	{
		pitchNode = GetNode<Node3D>("Pitch");
		camera = pitchNode.GetNode<Camera3D>("Camera3D");

		// Start with a reasonable top-down-ish angle
		pitchNode.RotationDegrees = new Vector3(-40, 0, 0);
		camera.Position = new Vector3(0, 0, currentZoom);
		camera.LookAt(GlobalPosition, Vector3.Up);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb)
		{
			if (mb.ButtonIndex == MouseButton.Right)
				dragging = mb.Pressed;
			else if (mb.ButtonIndex == MouseButton.WheelUp)
				Zoom(-zoomSpeed);
			else if (mb.ButtonIndex == MouseButton.WheelDown)
				Zoom(zoomSpeed);
		}
		else if (@event is InputEventMouseMotion mm && dragging)
		{
			// Yaw: rotate the whole rig around its own Y axis (world up)
			RotateY(-mm.Relative.X * rotateSpeed);

			// Pitch: rotate only the inner Pitch node around local X
			Vector3 pitchRot = pitchNode.RotationDegrees;
			pitchRot.X = Mathf.Clamp(pitchRot.X - mm.Relative.Y * rotateSpeed * 57.3f, minPitch, maxPitch);
			pitchNode.RotationDegrees = pitchRot;
		}
	}

	void Zoom(float delta)
	{
		currentZoom = Mathf.Clamp(currentZoom + delta, minZoom, maxZoom);
		camera.Position = new Vector3(0, 0, currentZoom);
	}
}
