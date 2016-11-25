public struct CollisionInfo
{
	public bool above, below;
	public bool left, right;
	public bool climbingSlope;
	public bool descendingSlope;

	public float slopeAngle, slopeAngleOld;
	public void Reset()
	{
		descendingSlope = climbingSlope = above = below = left = right = false;
		slopeAngleOld = slopeAngle;
		slopeAngle = 0;
	}
}