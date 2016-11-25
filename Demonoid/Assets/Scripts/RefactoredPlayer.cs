//using UnityEngine;
//using System.Collections;
//using System;

//[RequireComponent(typeof(Controller2D))]
//public class RefactoredPlayer : MonoBehaviour
//{
//	public float JUMP_HEIGHT = 3.5f;
//	public float TIME_TO_JUMP_APEX = .4f;
//	public float MOVE_SPEED = 12;
//	public float MOVE_ACCELERATION_TIME_AIRBORNE = .2f;
//	public float MOVE_ACCELERATION_TIME_GROUNDED = .07f;

//	private float JUMP_SPEED;
//	private float GRAVITY;

//	public RefactoredPlayer()
//	{
//	}

//	float velocityXSmoothing;
//	Vector3 velocity;
//	Controller2D controller;

//	private bool IsGrounded
//	{
//		get
//		{
//			return controller.collisionInfo.below;
//		}
//	}

//	void Start()
//	{
//		controller = GetComponent<Controller2D>();
//		GRAVITY = -2 * JUMP_HEIGHT / (TIME_TO_JUMP_APEX * TIME_TO_JUMP_APEX);
//		JUMP_SPEED = -GRAVITY * TIME_TO_JUMP_APEX;
//	}

//	void Update()
//	{
//		var collisions = controller.CheckCollisions();
//		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

//		if (controller.collisionInfo.below || controller.collisionInfo.above)
//			velocity.y = 0;
//		if (controller.collisionInfo.left || controller.collisionInfo.right)
//			velocity.x = 0;

//		if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
//		{
//			velocity.y = JUMP_SPEED;
//		}

//		float targetVelocityX = input.x * MOVE_SPEED;
//		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, IsGrounded ? MOVE_ACCELERATION_TIME_GROUNDED : MOVE_ACCELERATION_TIME_AIRBORNE);

//		velocity.y += GRAVITY * Time.deltaTime;
//		controller.Move(velocity * Time.deltaTime);
//	}
//}
