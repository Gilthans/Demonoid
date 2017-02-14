using System;
using System.Timers;
using UnityEngine;

namespace Assets.Scripts
{
	public class StupidAiInputSource : IInputSource
	{
		private Vector2 _movementVector;
		public StupidAiInputSource()
		{
			_movementVector = new Vector2(1, 0);
			var timer = new Timer(1000);
			timer.Elapsed += Timer_Elapsed;
			timer.Start();

		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			_movementVector.x *= -1;
		}

		public bool ShouldJump
		{
			get
			{
				return false;
			}
		}

		public Vector2 GetMovementVector()
		{
			return _movementVector;
		}
	}
}