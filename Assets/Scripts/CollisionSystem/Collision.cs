using System;
using UnityEngine;
using UnityEngine.Android;

public class Collision
{
    public GameObject obj;
    public GameObject other;
    public Vector3 point;
    public Vector3 normal;
    public float time;

    public Collision(GameObject thisObj, GameObject otherObj, Vector3 collisionPoint, Vector3 collisionNormal, float collisionTime)
    {
        obj = thisObj;
        other = otherObj;
        point = collisionPoint;
        normal = collisionNormal.normalized;
        time = collisionTime;
    }
}
