using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
	private const float SKIN_WIDTH = .015f;

	public LayerMask collisionMask;

	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	float MAX_CLIMB_ANGLE = 60;
	float MAX_DESCEND_ANGLE = 80;

	float horizontalRaySpacing = 0;
	float verticalRaySpacing = 0;

	private BoxCollider2D _collider;
	private Animator _animator;

	private bool _isFacingRight = true;

	private RaycastOrigins _raycastOrigins;

	public CollisionInfo collisionInfo;

	public void Move(Vector3 velocity)
	{
		UpdateRaycastOrigins();
		collisionInfo.Reset();

		if (velocity.y < 0)
			DescendSlope(ref velocity);
		if (velocity.x != 0)
			HorizontalCollisions(ref velocity);
		if (velocity.y != 0)
			VerticalCollisions(ref velocity);

		SetAnimatorProperties(velocity);

		transform.Translate(velocity);

		if(transform.position.y < -8)
		{
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.name);
		}
	}

	private void SetAnimatorProperties(Vector3 velocity)
	{
		if (_animator == null)
			return;

		_animator.SetBool("Ground", collisionInfo.below);
		_animator.SetBool("Crouch", false);
		_animator.SetFloat("Speed", Math.Abs(velocity.x));
		if ((velocity.x < 0 && _isFacingRight) ||
			(velocity.x > 0 && !_isFacingRight))
		{
			// TODO: There is a bug when walking into a wall where it momentarily flips
			FlipDirection(velocity);
		}
		_animator.SetFloat("vSpeed", velocity.y);
	}

	private void FlipDirection(Vector3 velocity)
	{
		_isFacingRight = velocity.x > 0;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Start()
	{
		_collider = GetComponent<BoxCollider2D>();
		_animator = GetComponent<Animator>();
		CalculateRaySpacing();
	}

	private void VerticalCollisions(ref Vector3 velocity)
	{
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + SKIN_WIDTH;

		for (int i = 0; i < verticalRayCount; i++)
		{
			Vector2 rayOrigin = (directionY < 0) ? _raycastOrigins.bottomLeft : _raycastOrigins.topLeft;
			var currentRaySpacing = Vector2.right * (verticalRaySpacing * i + velocity.x);
			rayOrigin += currentRaySpacing;

			Debug.DrawRay(rayOrigin, 2 * rayLength * Vector2.up * directionY, Color.green);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			if (hit)
			{
				velocity.y = (hit.distance - SKIN_WIDTH) * directionY;
				rayLength = hit.distance;

				if (collisionInfo.climbingSlope)
					velocity.x = velocity.y / Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);

				if (directionY > 0)
					collisionInfo.above = true;
				if (directionY < 0)
					collisionInfo.below = true;
			}
		}

		if (collisionInfo.climbingSlope)
		{
			float directionX = Mathf.Sign(velocity.x);
			rayLength = Mathf.Abs(velocity.x) + SKIN_WIDTH;
			Vector2 rayOrigin = (directionX == -1 ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight) + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			if (hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != collisionInfo.slopeAngle)
				{
					velocity.x = (hit.distance - SKIN_WIDTH) * directionX;
					collisionInfo.slopeAngle = slopeAngle;
				}
			}
		}
	}

	private void HorizontalCollisions(ref Vector3 velocity)
	{
		float directionX = Mathf.Sign(velocity.x);
		float rayLength = Mathf.Abs(velocity.x) + SKIN_WIDTH;

		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (directionX < 0) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight;
			var currentRaySpacing = Vector2.up * (horizontalRaySpacing * i);
			rayOrigin += currentRaySpacing;

			Debug.DrawRay(rayOrigin, 2 * rayLength * Vector2.right * directionX, Color.green);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			if (hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

				if (i == 0 && slopeAngle <= MAX_CLIMB_ANGLE)
				{
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisionInfo.slopeAngleOld)
					{
						distanceToSlopeStart = hit.distance - SKIN_WIDTH;
						velocity.x -= distanceToSlopeStart * directionX;
					}

					ClimbSlope(ref velocity, slopeAngle);

					velocity.x += distanceToSlopeStart * directionX;
				}

				if (collisionInfo.climbingSlope && slopeAngle <= MAX_CLIMB_ANGLE)
					continue;

				velocity.x = (hit.distance - SKIN_WIDTH) * directionX;
				rayLength = hit.distance;

				if (collisionInfo.climbingSlope)
					velocity.y = Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);

				if (directionX > 0)
					collisionInfo.right = true;
				if (directionX < 0)
					collisionInfo.left = true;
			}

		}
	}

	private void ClimbSlope(ref Vector3 velocity, float slopeAngle)
	{
		var slopeAngleRadians = slopeAngle * Mathf.Deg2Rad;
		float moveDistance = Mathf.Abs(velocity.x);
		float climbVelocityY = moveDistance * Mathf.Sin(slopeAngleRadians);
		if (velocity.y > climbVelocityY)
			print("Jumping on slope");
		else
		{
			velocity.y = climbVelocityY;
			velocity.x = moveDistance * Mathf.Cos(slopeAngleRadians) * Mathf.Sign(velocity.x);

			collisionInfo.below = true;
			collisionInfo.climbingSlope = true;
			collisionInfo.slopeAngle = slopeAngle;
		}
	}

	private void DescendSlope(ref Vector3 velocity)
	{
		float directionX = Mathf.Sign(velocity.x);
		Vector2 rayOrigin = directionX == -1 ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);
		if (hit)
		{
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			var slopeAngleRadians = slopeAngle * Mathf.Deg2Rad;
			if (slopeAngle != 0 && slopeAngle <= MAX_DESCEND_ANGLE)
			{
				if (Mathf.Sign(hit.normal.x) == directionX)
				{
					if (hit.distance - SKIN_WIDTH <= Mathf.Tan(slopeAngleRadians) * Mathf.Abs(velocity.x))
					{
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = moveDistance * Mathf.Sin(slopeAngleRadians);
						velocity.x = Mathf.Cos(slopeAngleRadians) * moveDistance * Mathf.Sign(velocity.x);
						velocity.y -= descendVelocityY;

						collisionInfo.slopeAngle = slopeAngle;
						collisionInfo.descendingSlope = true;
						collisionInfo.below = true;
					}
				}
			}
		}
	}

	private void UpdateRaycastOrigins()
	{
		Bounds bounds = GetActualBounds();

		_raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		_raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		_raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		_raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	private void CalculateRaySpacing()
	{
		Bounds bounds = GetActualBounds();

		if (horizontalRayCount < 2)
			throw new ArgumentException("Must have at least 2 rays", "horizontalRayCount");
		if (verticalRayCount < 2)
			throw new ArgumentException("Must have at least 2 rays", "verticalRayCount");

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	private Bounds GetActualBounds()
	{
		Bounds bounds = _collider.bounds;
		bounds.Expand(-2 * SKIN_WIDTH);
		return bounds;
	}

	struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
