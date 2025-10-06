using UnityEngine;
public class BoxCollider : ICollider
{
    public Vector3 maxCorner;
    public Vector3 minCorner;
    public Vector3[] corners = new Vector3[4];
    public Vector3 scale = Vector3.one;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        maxCorner = new Vector3(transform.lossyScale.x * scale.x / 2, 0, transform.lossyScale.z * scale.z / 2);
        minCorner = new Vector3(-transform.lossyScale.x * scale.x / 2, 0, -transform.lossyScale.z * scale.z / 2);
        UpdateCorners();
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
        return Quaternion.Inverse(transform.rotation) * (vec - new Vector3(transform.position.x, 0, transform.position.z));
    }

    public Vector3 LocalToWorld(Vector3 vec)
    {
        return transform.rotation * vec + new Vector3 (transform.position.x, 0, transform.position.z);
    }

    void OnValidate()
    {
        maxCorner = new Vector3(transform.lossyScale.x * scale.x / 2, 0, transform.lossyScale.z * scale.z / 2);
        minCorner = new Vector3(-transform.lossyScale.x * scale.x / 2, 0, -transform.lossyScale.z * scale.z / 2);
        UpdateCorners();
    }

    private void UpdateCorners()
    {
        corners[0] = new Vector3(maxCorner.x, 0, maxCorner.z);
        corners[1] = new Vector3(maxCorner.x, 0, minCorner.z);
        corners[2] = new Vector3(minCorner.x, 0, minCorner.z);
        corners[3] = new Vector3(minCorner.x, 0, maxCorner.z);
    }

    void OnDrawGizmosSelected()
    {
        Vector3[] rotatedCorners = LocalToWorld(corners);
        Vector3 up = Vector3.up * 0.5f;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rotatedCorners[0]+up, rotatedCorners[1]+up);
        Gizmos.DrawLine(rotatedCorners[1]+up, rotatedCorners[2]+up);
        Gizmos.DrawLine(rotatedCorners[2]+up, rotatedCorners[3]+up);
        Gizmos.DrawLine(rotatedCorners[3]+up, rotatedCorners[0]+up);
    }
}
