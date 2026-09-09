using Godot;
using System;

[GlobalClass]
public partial class Entity : Node
{
	protected const int defaultHealth = 3;
	
	[Signal]
	public delegate void DeadEventHandler(Entity entity);
	public EntityGUI gui;
	int _health;
	public int health
	{
		get => _health;
		set 
		{
			if (value <= 0)
			{
				EmitSignal(SignalName.Dead, this);
			}
		}
	}
	Vector2I position;
	public Direction direction;
	public bool damagable;
	public Faction faction;


	public Entity(Faction faction, int health)
	{
		Name = faction.ToString();
		this.faction = faction;
		this.health = health;
		damagable = true;
		gui = EntityGUI.getEntityGUI();
		AddChild(gui);
	}


	public void takeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			EmitSignal(SignalName.Dead, this);
		}
	}
	public bool checkIfDeadAfter(int damage)
	{
		return health - damage <= 0;
	}

}
