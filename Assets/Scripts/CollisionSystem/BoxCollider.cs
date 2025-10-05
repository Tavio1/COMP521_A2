using UnityEngine;
public class BoxCollider : ICollider
{
    public Vector3 maxCorner;
    public Vector3 minCorner;
    private Vector3[] corners = new Vector3[4];
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        maxCorner = new Vector3(transform.localScale.x / 2, 0, transform.localScale.z / 2);
        minCorner = new Vector3(-transform.localScale.x / 2, 0, -transform.localScale.z / 2);

        corners[0] = new Vector3(transform.localScale.x / 2, 0, transform.localScale.z / 2);
        corners[1] = new Vector3(transform.localScale.x / 2, 0, -transform.localScale.z / 2);
        corners[2] = new Vector3(-transform.localScale.x / 2, 0, -transform.localScale.z / 2);
        corners[3] = new Vector3(-transform.localScale.x / 2, 0, transform.localScale.z / 2);
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



    void OnDrawGizmosSelected()
    {
        Vector3[] rotatedCorners = LocalToWorld(corners);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rotatedCorners[0], rotatedCorners[1]);
        Gizmos.DrawLine(rotatedCorners[1], rotatedCorners[2]);
        Gizmos.DrawLine(rotatedCorners[2], rotatedCorners[3]);
        Gizmos.DrawLine(rotatedCorners[3], rotatedCorners[0]);
    }
}
