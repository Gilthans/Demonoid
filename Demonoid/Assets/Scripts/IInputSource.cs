using UnityEngine;

public interface IInputSource
{
	Vector2 GetMovementVector();

	bool ShouldJump { get; }
}