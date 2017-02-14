using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

[RequireComponent(typeof(Controller2D))]
public class Player : MovingCreature
{
	public Player() : base(new PlayerInputSource())
	{
	}

	public override void Update()
	{
		base.Update();

		if (transform.position.y < -8)
			Die();

		if (this.controller.collisionInfo.Objects.Any(o => o.layer == LayerMask.NameToLayer("Enemies")))
			Die();
	}

	private static void Die()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}
}
