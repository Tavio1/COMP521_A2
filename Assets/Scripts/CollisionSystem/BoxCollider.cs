using UnityEngine;

public class BoxCollider : ICollider
{
    public Vector3 maxCorner;
    public Vector3 minCorner;
    public Vector3[] vertices = new Vector3[4];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        maxCorner = new Vector3(transform.position.x + transform.localScale.x / 2, 0, transform.position.z + transform.localScale.z / 2);
        minCorner = new Vector3(transform.position.x - transform.localScale.x / 2, 0, transform.position.z - transform.localScale.z / 2);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(maxCorner, new Vector3(maxCorner.x, 0, minCorner.z));
        Gizmos.DrawLine(maxCorner, new Vector3(minCorner.x, 0, maxCorner.z));
        Gizmos.DrawLine(minCorner, new Vector3(maxCorner.x, 0, minCorner.z));
        Gizmos.DrawLine(minCorner, new Vector3(minCorner.x, 0, maxCorner.z));
    }
}
