using UnityEngine;

public class Collision
{
    public GameObject obj;
    public GameObject other;
    public Vector3 point;
    public Vector3 normal;

    public Collision(GameObject thisObj, GameObject otherObj, Vector3 collisionPoint, Vector3 collisionNormal)
    {
        obj = thisObj;
        other = otherObj;
        point = collisionPoint;
        normal = collisionNormal;
    }
}
