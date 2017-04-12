using System;
using System.Timers;
using UnityEngine;

namespace Assets.Scripts
{
	public class AiInputSource : IInputSource
	{
		private Vector2 _movementVector;

		public Vector2 MovementVector
		{
			get { return _movementVector; }
			set { _movementVector = value.normalized; }
		}

		public AiInputSource()
		{
		}

		public void SwitchDirection()
		{
			_movementVector.x *= -1;
		}

		public bool ShouldJump
		{
			get;
			set;
		}

		public Vector2 GetMovementVector()
		{
			return _movementVector;
		}
	}
}