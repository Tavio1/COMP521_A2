using System.Collections.Generic;
using UnityEngine;

public class RigidBodyManager : MonoBehaviour, IGameSystem
{
    public static RigidBodyManager instance;
    private List<RigidBody> rigidBodies = new List<RigidBody>();
    public GameObject gravity;
    public float maxVelocity;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void tick()
    {
        foreach (RigidBody rb in rigidBodies)
        {
            if (rb.gravity)
                rb.acceleration += gravity.transform.position;
            rb.UpdateRigidBody(Time.fixedDeltaTime);
        }
    }

    public void AddRigidBody(RigidBody rb)
    {
        if (!rigidBodies.Contains(rb))
        {
            rigidBodies.Add(rb);
        }
    }
    
    public void RemoveRigidBody(RigidBody rb)
    {
        if (rigidBodies.Contains(rb))
        {
            rigidBodies.Remove(rb);
        }
    }
}
