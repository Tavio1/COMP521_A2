using UnityEngine;

// Defines a circle
public class CircleCollider : ICollider
{
    public float radius; // Radius of the collider

    // Draws the collider
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
