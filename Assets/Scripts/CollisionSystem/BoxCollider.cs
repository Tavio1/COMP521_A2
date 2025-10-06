using UnityEngine;
public class BoxCollider : ICollider
{
    public Vector3 maxCorner;
    public Vector3 minCorner;
    // private Vector3[] corners = new Vector3[4];
    public Vector3 scale = Vector3.one;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        maxCorner = new Vector3(transform.lossyScale.x * scale.x / 2, 0, transform.lossyScale.z * scale.z / 2);
        minCorner = new Vector3(-transform.lossyScale.x * scale.x / 2, 0, -transform.lossyScale.z * scale.z / 2);
    }

    public Vector3[] LocalToWorld(Vector3[] vecs)
    {
        Vector3[] output = new Vector3[vecs.Length];
        for (int i = 0; i < vecs.Length; i++)
        {
            output[i] = LocalToWorld(vecs[i]);
        }

        return output;
    }

    public Vector3 WorldToLocal(Vector3 vec)
    {
        return Quaternion.Inverse(transform.rotation) * (vec - transform.position);
    }

    public Vector3 LocalToWorld(Vector3 vec)
    {
        return transform.rotation * vec + transform.position;
    }

    void OnValidate()
    { 
        maxCorner = new Vector3(transform.lossyScale.x * scale.x / 2, 0, transform.lossyScale.z * scale.z / 2);
        minCorner = new Vector3(-transform.lossyScale.x * scale.x / 2, 0, -transform.lossyScale.z * scale.z / 2);
    }

    void OnDrawGizmosSelected()
    {
        Vector3[] corners = { new Vector3(maxCorner.x, 0.5f, maxCorner.z),
                              new Vector3(maxCorner.x, 0.5f, minCorner.z),
                              new Vector3(minCorner.x, 0.5f, minCorner.z),
                              new Vector3(minCorner.x, 0.5f, maxCorner.z)};
        Vector3[] rotatedCorners = LocalToWorld(corners);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rotatedCorners[0], rotatedCorners[1]);
        Gizmos.DrawLine(rotatedCorners[1], rotatedCorners[2]);
        Gizmos.DrawLine(rotatedCorners[2], rotatedCorners[3]);
        Gizmos.DrawLine(rotatedCorners[3], rotatedCorners[0]);
    }
}
