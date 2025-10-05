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
        Vector3 v = rb.lastPos - rb.transform.position;
        Vector3 vNormalized = v.normalized;
        Vector3 D = Vector3.Dot(difference, vNormalized) * vNormalized;
        Vector3 F = difference - D;
        float tLength = Mathf.Sqrt(Mathf.Pow(radius + other.radius, 2) - F.sqrMagnitude);
        Vector3 T = new Vector3(-vNormalized.z, 0, vNormalized.x).normalized * tLength;
        Vector3 collisionPoint = rb.lastPos + D - T;
        Vector3 collisionNormal = (other.transform.position - collisionPoint).normalized;
        float collisionTime = (D.magnitude - tLength) / v.magnitude * Time.deltaTime;

        // Create collision
        Collision collision = new Collision(this.gameObject, other.gameObject, collisionPoint, collisionNormal, collisionTime);

        return collision;
    }
}
