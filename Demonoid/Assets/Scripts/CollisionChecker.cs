//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace Assets.Scripts
//{
//	public class CollisionChecker
//	{
//		private CollisionInfo _collisionInfo = new CollisionInfo();
//		private const float SKIN_WIDTH = 0.15f;

//		internal CollisionInfo CheckCollisions(Vector2 location)
//		{
//			_collisionInfo.Reset();
//			CheckHorizontalCollisions(location);

//			return _collisionInfo;
//		}

//		private CheckHorizontalCollisions(Vector2 location)
//		{
//			float directionX = Mathf.Sign(velocity.x);
//			float rayLength = Mathf.Abs(velocity.x) + SKIN_WIDTH;

//			for (int i = 0; i < horizontalRayCount; i++)
//			{
//				Vector2 rayOrigin = (directionX < 0) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight;
//				var currentRaySpacing = Vector2.up * (horizontalRaySpacing * i);
//				rayOrigin += currentRaySpacing;

//				Debug.DrawRay(rayOrigin, 2 * rayLength * Vector2.right * directionX, Color.green);

//				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
//				if (hit)
//				{
//					float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

//					if (i == 0 && slopeAngle <= MAX_CLIMB_ANGLE)
//					{
//						float distanceToSlopeStart = 0;
//						if (slopeAngle != collisionInfo.slopeAngleOld)
//						{
//							distanceToSlopeStart = hit.distance - SKIN_WIDTH;
//							velocity.x -= distanceToSlopeStart * directionX;
//						}

//						ClimbSlope(ref velocity, slopeAngle);

//						velocity.x += distanceToSlopeStart * directionX;
//					}

//					if (collisionInfo.climbingSlope && slopeAngle <= MAX_CLIMB_ANGLE)
//						continue;

//					velocity.x = (hit.distance - SKIN_WIDTH) * directionX;
//					rayLength = hit.distance;

//					if (collisionInfo.climbingSlope)
//						velocity.y = Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);

//					if (directionX > 0)
//						collisionInfo.right = true;
//					if (directionX < 0)
//						collisionInfo.left = true;
//				}

//			}
//		}
//	}
//}
