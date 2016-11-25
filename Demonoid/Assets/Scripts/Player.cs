using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Controller2D))]
public class Player : MovingCreature
{
	public Player() : base(new PlayerInputSource())
	{
	}
}
