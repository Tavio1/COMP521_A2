using UnityEngine;

// Defines an OBB
public class BoxCollider : ICollider
{
    public Vector3 maxCorner; // Max corner in local coords
    public Vector3 minCorner; // Min corner in local coords
    public Vector3[] corners = new Vector3[4]; // Helper list of all corners in local coords
    public Vector3 scale = Vector3.one; // Collider scale


    public override void Start()
    {
        base.Start();

        // Set max and min corners using transform scale and collider scale
        maxCorner = new Vector3(transform.lossyScale.x * scale.x / 2, 0, transform.lossyScale.z * scale.z / 2);
        minCorner = new Vector3(-transform.lossyScale.x * scale.x / 2, 0, -transform.lossyScale.z * scale.z / 2);
        UpdateCorners();
    }

    // Converts a list of local coordinates to world coordinates (ignoring transform scale operations)
    public Vector3[] LocalToWorld(Vector3[] vecs)
    {
        Vector3[] output = new Vector3[vecs.Length];
        for (int i = 0; i < vecs.Length; i++)
        {
            output[i] = LocalToWorld(vecs[i]);
        }

        return output;
    }

    // Converts a point in world coordinates to local coordinates (ignoring transform scale operations)
    public Vector3 WorldToLocal(Vector3 vec)
    {
        return Quaternion.Inverse(transform.rotation) * (vec - new Vector3(transform.position.x, 0, transform.position.z));
    }

    // Converts a point in local coordinates to world coordinates (ignoring transform scale operations)
    public Vector3 LocalToWorld(Vector3 vec)
    {
        return transform.rotation * vec + new Vector3(transform.position.x, 0, transform.position.z);
    }

    // Updates max and min corners anytime there is an update in the editor
    void OnValidate()
    {
        maxCorner = new Vector3(transform.lossyScale.x * scale.x / 2, 0, transform.lossyScale.z * scale.z / 2);
        minCorner = new Vector3(-transform.lossyScale.x * scale.x / 2, 0, -transform.lossyScale.z * scale.z / 2);
        UpdateCorners();
    }

    // Sets corners based on the max and min
    private void UpdateCorners()
    {
        corners[0] = new Vector3(maxCorner.x, 0, maxCorner.z);
        corners[1] = new Vector3(maxCorner.x, 0, minCorner.z);
        corners[2] = new Vector3(minCorner.x, 0, minCorner.z);
        corners[3] = new Vector3(minCorner.x, 0, maxCorner.z);
    }

    // Draws the collider
    void OnDrawGizmosSelected()
    {
        Vector3[] rotatedCorners = LocalToWorld(corners);
        Vector3 up = Vector3.up * 0.5f;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rotatedCorners[0] + up, rotatedCorners[1] + up);
        Gizmos.DrawLine(rotatedCorners[1] + up, rotatedCorners[2] + up);
        Gizmos.DrawLine(rotatedCorners[2] + up, rotatedCorners[3] + up);
        Gizmos.DrawLine(rotatedCorners[3] + up, rotatedCorners[0] + up);
    }
}
