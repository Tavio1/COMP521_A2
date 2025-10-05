using UnityEngine;

public class PinballCollider : CircleCollider
{
    private RigidBody rb;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<RigidBody>();
    }

    public bool Intersects(ICollider other, out Collision collision)
    {
        collision = null;

        switch (other)
        {
            case CircleCollider circle:
                collision = IntersectsWithCircle(circle);
                return collision != null;
            case BoxCollider square:
                collision = IntersectsWithSquare(square);
                return collision != null;
            // case TriangleCollider triangle:
            //     return IntersectsWithTriangle(triangle);
            default:
                throw new System.Exception("Unknown collider type");
        }
    }

    private Collision IntersectsWithCircle(CircleCollider other)
    {
        Vector3 difference = other.transform.position - transform.position;

        // No collision, return null
        if (difference.magnitude > (radius + other.radius)) return null;

        // Find collision point, normal, and time
        Vector3 v = rb.transform.position - rb.lastPos;
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

    private Collision IntersectsWithSquare(BoxCollider other)
    {
        float sphereMaxX = transform.position.x + radius;
        float sphereMinX = transform.position.x - radius;
        float sphereMaxZ = transform.position.z + radius;
        float sphereMinZ = transform.position.z - radius;

        // No collision, return null
        if (sphereMaxX < other.minCorner.x || sphereMinX > other.maxCorner.x ||
            sphereMaxZ < other.minCorner.z || sphereMinZ > other.maxCorner.z)
            return null;

        // Find collision point, normal, and time
        Vector3 collisionPoint = Vector3.zero;
        Vector3 collisionNormal = Vector3.zero;
        float collisionTime = 1; // Currently on 0-1 from beginnging to end, will convert to absolute time after

        Vector3 v = rb.transform.position - rb.lastPos;

        // Check each side individually, find the earliest collision

        // Left 
        if (rb.lastPos.x + radius <= other.minCorner.x && sphereMaxX >= other.minCorner.x && v.x > 0)
        {
            float t = (other.minCorner.x - (rb.lastPos.x + radius)) / v.x;
            if (t >= 0 && t <= 1 && t < collisionTime)
            {
                collisionTime = t;
                collisionNormal = Vector3.left;
            }
        }

        // Right
        if (rb.lastPos.x - radius >= other.maxCorner.x && sphereMinX <= other.maxCorner.x && v.x < 0)
        {
            float t = (other.maxCorner.x - (rb.lastPos.x - radius)) / v.x;
            if (t >= 0 && t <= 1 && t < collisionTime)
            {
                collisionTime = t;
                collisionNormal = Vector3.right;
            }
        }

        // Bottom 
        if (rb.lastPos.z + radius <= other.minCorner.z && sphereMaxZ >= other.minCorner.z && v.z > 0)
        {
            float t = (other.minCorner.z - (rb.lastPos.z + radius)) / v.z;
            if (t >= 0 && t <= 1 && t < collisionTime)
            {
                collisionTime = t;
                collisionNormal = Vector3.back;
            }
        }

        // Top
        if (rb.lastPos.z - radius >= other.maxCorner.z && sphereMinZ <= other.maxCorner.z && v.z < 0)
        {
            float t = (other.maxCorner.z - (rb.lastPos.z - radius)) / v.z;
            if (t >= 0 && t <= 1 && t < collisionTime)
            {
                collisionTime = t;
                collisionNormal = Vector3.forward;
            }
        }

        // Create collision
        collisionPoint = Vector3.Lerp(rb.lastPos, rb.transform.position, collisionTime);
        collisionTime = collisionTime * Time.fixedDeltaTime;
        Collision collision = new Collision(this.gameObject, other.gameObject, collisionPoint, collisionNormal, collisionTime);

        return collision;
    }
}
