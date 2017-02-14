using System.Collections.Generic;
using UnityEngine;

public struct CollisionInfo
{
	public bool above, below;
	public bool left, right;
	public bool climbingSlope;
	public bool descendingSlope;

	public List<GameObject> Objects;

	public float slopeAngle, slopeAngleOld;
	public void Reset()
	{
		if (Objects == null)
			Objects = new List<GameObject>();

		Objects.Clear();
		descendingSlope = climbingSlope = above = below = left = right = false;
		slopeAngleOld = slopeAngle;
		slopeAngle = 0;
	}
}