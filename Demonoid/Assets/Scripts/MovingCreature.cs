using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public abstract class MovingCreature : MonoBehaviour
{
	private float TIME_TO_JUMP_APEX = .4f;

	public float JUMP_HEIGHT = 3.5f;
	public float MOVE_SPEED = 12;
	public float MOVE_ACCELERATION_TIME_AIRBORNE = .2f;
	public float MOVE_ACCELERATION_TIME_GROUNDED = .07f;

	private float JUMP_SPEED;
	private Vector2 GRAVITY;
	protected IInputSource _InputSource;

	protected MovingCreature(IInputSource inputSource = null)
	{
		_InputSource = inputSource;
	}

	protected float velocityXSmoothing;
	protected Vector2 velocity;
	protected Controller2D controller;
	private bool _isFacingRight = true;

	private bool IsGrounded
	{
		get
		{
			return controller.collisionInfo.below;
		}
	}

	public virtual void Start()
	{
		controller = GetComponent<Controller2D>();
		GRAVITY = new Vector2(0, -2 * JUMP_HEIGHT / (TIME_TO_JUMP_APEX * TIME_TO_JUMP_APEX));
		JUMP_SPEED = -GRAVITY.y * TIME_TO_JUMP_APEX;
	}

	public virtual void Update()
	{
		Vector2 input = _InputSource.GetMovementVector();

		if (controller.collisionInfo.below || controller.collisionInfo.above)
			velocity.y = 0;
		//if (controller.collisionInfo.left || controller.collisionInfo.right)
		//	velocity.x = 0;

		if (_InputSource.ShouldJump && IsGrounded)
		{
			velocity.y = JUMP_SPEED;
		}

		if ((input.x > 0 && !_isFacingRight) ||
			(input.x < 0 && _isFacingRight))
		{
			FlipDirection(input);
		}

		float targetVelocityX = input.x * MOVE_SPEED;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, IsGrounded ? MOVE_ACCELERATION_TIME_GROUNDED : MOVE_ACCELERATION_TIME_AIRBORNE);

		velocity += GRAVITY * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}

	private void FlipDirection(Vector2 velocity)
	{
		_isFacingRight = velocity.x > 0;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
