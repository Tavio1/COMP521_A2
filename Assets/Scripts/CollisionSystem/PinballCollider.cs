using UnityEngine;

// Defines a pinball
public class PinballCollider : CircleCollider
{
    private RigidBody rb;

    // Get the rigidbody of the pinball
    public override void Start()
    {
        base.Start();
        rb = GetComponent<RigidBody>();
    }

    // Detect intersections with another collider, outputs the collision event if there is one
    public bool Intersects(ICollider other, out Collision collision)
    {
        collision = null;

        // Detect collision differently depending on the type of collider it is checking
        switch (other)
        {
            case PinballCollider otherPinball:
                collision = IntersectsWithCircle(otherPinball, true);
                return collision != null;
            case CircleCollider circle:
                collision = IntersectsWithCircle(circle);
                return collision != null;
            case BoxCollider square:
                collision = IntersectsWithSquare(square);
                return collision != null;
            default:
                throw new System.Exception("Unknown collider type");
        }
    }

    // Detects intersection with another circle
    private Collision IntersectsWithCircle(CircleCollider other, bool otherIsPinball = false)
    {
        Vector3 difference = other.transform.position - transform.position;

        // No collision, return null
        if (difference.magnitude > (radius + other.radius)) return null;

        // If doing pinball-pinball collision, simplify to one pinball moving using the velocities of both
        Vector3 v = rb.velocity;
        if (otherIsPinball) v -= other.GetComponent<RigidBody>().velocity;

        
        // Find collision point, normal, and time
        Vector3 vNormalized = v.normalized;
        Vector3 C = other.transform.position - rb.lastPos;
        Vector3 D = Vector3.Dot(C, vNormalized) * vNormalized;
        Vector3 F = C - D;
        float tLength = Mathf.Sqrt(Mathf.Pow(radius + other.radius, 2) - F.sqrMagnitude);
        Vector3 T = -vNormalized * tLength;
        Vector3 collisionPoint = rb.lastPos + D + T;
        Vector3 collisionNormal = (collisionPoint - other.transform.position).normalized;
        float collisionTime = ((D.magnitude - tLength) / v.magnitude) * Time.fixedDeltaTime;

        // Create collision
        Collision collision = new Collision(this.gameObject, other.gameObject, collisionPoint, collisionNormal, collisionTime);

        return collision;
    }

    // Detects intersection with an OBB
    private Collision IntersectsWithSquare(BoxCollider other)
    {
        // Gets positions in the local coordinates
        Vector3 rotatedPos = other.WorldToLocal(transform.position);
        Vector3 rotatedLastPos = other.WorldToLocal(rb.lastPos);

        float sphereMaxX = rotatedPos.x + radius;
        float sphereMinX = rotatedPos.x - radius;
        float sphereMaxZ = rotatedPos.z + radius;
        float sphereMinZ = rotatedPos.z - radius;

        // No collision, return null
        if (sphereMaxX < other.minCorner.x || sphereMinX > other.maxCorner.x ||
            sphereMaxZ < other.minCorner.z || sphereMinZ > other.maxCorner.z)
            return null;

        // Find collision point, normal, and time
        Vector3 collisionPoint = rotatedLastPos;
        Vector3 collisionNormal = Vector3.zero;
        float collisionTime = 1; // Currently on 0-1 from beginnging to end, will convert to absolute time after

        // If pinball was already inside the box, return normal of closest edge
        if ((rotatedLastPos.x >= other.minCorner.x - radius && rotatedLastPos.x <= other.maxCorner.x + radius) &&
            (rotatedLastPos.z >= other.minCorner.z - radius && rotatedLastPos.z <= other.maxCorner.z + radius))
        {
            // Distances to each of the 4 sides
            float dxMin = rotatedLastPos.x - other.minCorner.x;   // left face
            float dxMax = other.maxCorner.x - rotatedLastPos.x;   // right face
            float dzMin = rotatedLastPos.z - other.minCorner.z;   // bottom face
            float dzMax = other.maxCorner.z - rotatedLastPos.z;   // top face

            // Find the smallest distance, store the relevant collision normal
            float minDist = dxMin;
            collisionNormal = Vector3.left;

            if (dxMax < minDist) { minDist = dxMax; collisionNormal = Vector3.right; }
            if (dzMin < minDist) { minDist = dzMin; collisionNormal = Vector3.back; }
            if (dzMax < minDist) { minDist = dzMax; collisionNormal = Vector3.forward; }

            // Collision point was instant
            collisionPoint = rotatedLastPos;
            collisionTime = 0;
        }
        // Else, it started outside of the box so calculate the intersection
        else
        {
            Vector3 v = Quaternion.Inverse(other.transform.rotation) * rb.velocity;

            // Check each side individually, find the earliest collision

            // Left 
            if (rotatedLastPos.x + radius <= other.minCorner.x && sphereMaxX >= other.minCorner.x && v.x > 0)
            {
                float t = (other.minCorner.x - (rotatedLastPos.x + radius)) / v.x;
                if (t >= 0 && t <= 1 && t < collisionTime)
                {
                    collisionTime = t;
                    collisionNormal = Vector3.left;
                }
            }

            // Right
            if (rotatedLastPos.x - radius >= other.maxCorner.x && sphereMinX <= other.maxCorner.x && v.x < 0)
            {
                float t = (other.maxCorner.x - (rotatedLastPos.x - radius)) / v.x;
                if (t >= 0 && t <= 1 && t < collisionTime)
                {
                    collisionTime = t;
                    collisionNormal = Vector3.right;
                }
            }

            // Bottom 
            if (rotatedLastPos.z + radius <= other.minCorner.z && sphereMaxZ >= other.minCorner.z && v.z > 0)
            {
                float t = (other.minCorner.z - (rotatedLastPos.z + radius)) / v.z;
                if (t >= 0 && t <= 1 && t < collisionTime)
                {
                    collisionTime = t;
                    collisionNormal = Vector3.back;
                }
            }

            // Top
            if (rotatedLastPos.z - radius >= other.maxCorner.z && sphereMinZ <= other.maxCorner.z && v.z < 0)
            {
                float t = (other.maxCorner.z - (rotatedLastPos.z - radius)) / v.z;
                if (t >= 0 && t <= 1 && t < collisionTime)
                {
                    collisionTime = t;
                    collisionNormal = Vector3.forward;
                }
            }

            // Check corners
            for (int i = 0; i < other.corners.Length; i++)
            {
                Vector3 corner = other.corners[i];
                corner = new Vector3(corner.x, 0, corner.z);

                Vector3 difference = rotatedLastPos - corner;
                float a = Vector3.Dot(v, v);
                float b = 2f * Vector3.Dot(v, difference);
                float c = Vector3.Dot(difference, difference) - radius * radius;

                float discriminant = b * b - 4f * a * c;
                if (discriminant < 0f)
                    continue; // no intersection

                float sqrtD = Mathf.Sqrt(discriminant);
                float t1 = (-b - sqrtD) / (2f * a);
                float t2 = (-b + sqrtD) / (2f * a);

                // We want the earliest positive t in [0,1]
                float t = Mathf.Min(t1, t2);
                if (t < 0f || t > 1f)
                    continue;

                if (t < collisionTime)
                {
                    collisionTime = t;
                    Vector3 hitPos = rotatedLastPos + v * t;
                    collisionNormal = (hitPos - corner).normalized;
                }
            }
        }

        // Convert to world space
        collisionPoint = Vector3.Lerp(rotatedLastPos, rotatedPos, collisionTime);
        collisionPoint = other.LocalToWorld(collisionPoint);

        // Convert to world space
        collisionNormal = other.transform.rotation * collisionNormal;

        // Convert to actual time step, not a percent
        collisionTime = collisionTime * Time.fixedDeltaTime;

        // Create and return the collision
        Collision collision = new Collision(this.gameObject, other.gameObject, collisionPoint, collisionNormal, collisionTime);

        return collision;
    }
}
