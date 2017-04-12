using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI
{
	public class PatrollingAi : MovingCreature
	{
		public int TIME_BETWEEN_PATROLS = 1000;

		public List<Vector3> PatrolPositions = new List<Vector3>();

		private DateTime _waitStartTime;
		private int _targetPatrolPosition;
		private bool _isMoving = true;
		private PolygonCollider2D _fieldOfVision;
		private Collider2D _player;

		private AiInputSource _InputSource { get { return (AiInputSource)base._InputSource; } }

		public PatrollingAi() : base(new AiInputSource())
		{
		}

		public override void Start()
		{
			base.Start();
			_waitStartTime = DateTime.Now;

			_targetPatrolPosition = 0;
			var targetPosition = PatrolPositions[_targetPatrolPosition];
			_InputSource.MovementVector = targetPosition - this.transform.position;
			_fieldOfVision = GetComponentInChildren<PolygonCollider2D>();
			//_player = GetComponentInParent<Player>().GetComponent<Collider2D>();
		}

		public override void Update()
		{
			if (_fieldOfVision.IsTouchingLayers(LayerMask.NameToLayer("Player")))
			{
				Debug.Log("OMG");
				MOVE_SPEED = 15;
			}
			if (!_isMoving && IsWaitTimeElapsed())
			{
				_targetPatrolPosition++;
				if (_targetPatrolPosition >= PatrolPositions.Count)
					_targetPatrolPosition = 0;
				_isMoving = true;
				var targetPosition = PatrolPositions[_targetPatrolPosition];
				_InputSource.MovementVector = targetPosition - this.transform.position;
			}

			if (_isMoving && IsInPatrolPosition())
			{
				_waitStartTime = DateTime.Now;
				_InputSource.MovementVector = new Vector2(0, 0);
				_isMoving = false;
			}

			base.Update();
		}

		private bool IsInPatrolPosition()
		{
			var targetPosition = PatrolPositions[_targetPatrolPosition];
			var myPosition = this.transform.position;
			return (targetPosition - myPosition).sqrMagnitude < 0.1;
		}

		private bool IsWaitTimeElapsed()
		{
			return (DateTime.Now - _waitStartTime).TotalMilliseconds >= TIME_BETWEEN_PATROLS;
		}
	}
}
