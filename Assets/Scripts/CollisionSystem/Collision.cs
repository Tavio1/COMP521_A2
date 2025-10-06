using UnityEngine;

// Represents the data of a collision event
public class Collision
{
    public GameObject obj; // The object that caused the collision
    public GameObject other; // The object collided with
    public Vector3 point; // The point the object was in when it collided
    public Vector3 normal; // The normal of the collision
    public float time; // The time over the delta t that the collision occured

    public Collision(GameObject thisObj, GameObject otherObj, Vector3 collisionPoint, Vector3 collisionNormal, float collisionTime)
    {
        obj = thisObj;
        other = otherObj;
        point = collisionPoint;
        normal = collisionNormal.normalized;
        time = collisionTime;
    }
}
