using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public class StupidAi : MovingCreature
	{
		public int TIME_TO_SWAP = 1000;
		private DateTime MovementStartTime;
		private AiInputSource _InputSource { get { return (AiInputSource)base._InputSource; } }

		public StupidAi() : base(new AiInputSource())
		{
		}

		public override void Start()
		{
			base.Start();
			MovementStartTime = DateTime.Now;
			_InputSource.MovementVector = new Vector2(1, 0);
		}

		public override void Update()
		{
			var timePassed = (DateTime.Now - MovementStartTime);
			if (timePassed.TotalMilliseconds > TIME_TO_SWAP)
			{
				_InputSource.SwitchDirection();
				MovementStartTime += timePassed;
			}

			base.Update();
		}
	}
}
