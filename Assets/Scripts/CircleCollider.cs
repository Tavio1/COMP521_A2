using Unity.VisualScripting;
using UnityEngine;

public class CircleCollider : ICollider
{
    public float radius;

    public override Collision IntersectsWithCircle(CircleCollider other)
    {
        Vector3 difference = other.transform.position - transform.position;
        if (difference.magnitude > (radius + other.radius)) return null;

        Collision collision = new Collision(this.gameObject, other.gameObject,
            (transform.position + other.transform.position) / 2,
            (transform.position - other.transform.position).normalized);

        return collision;
    }
}
