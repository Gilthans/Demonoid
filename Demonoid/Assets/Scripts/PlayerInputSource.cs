using UnityEngine;

public class PlayerInputSource : IInputSource
{
	public Vector2 GetMovementVector()
	{
		return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
	}

	public bool ShouldJump
	{
		get { return Input.GetKeyDown(KeyCode.Space); }
	}
}