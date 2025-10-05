using UnityEngine;

public class PinballCollider : CircleCollider
{
    private RigidBody rb;

    void Start()
    {
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
            // case SquareCollider square:
            //     return IntersectsWithSquare(square);
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
}
