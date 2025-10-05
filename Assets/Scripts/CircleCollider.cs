using Unity.VisualScripting;
using UnityEngine;

public class CircleCollider : ICollider
{
    public float radius;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
