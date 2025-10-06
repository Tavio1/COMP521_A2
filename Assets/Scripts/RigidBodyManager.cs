using System.Collections.Generic;
using UnityEngine;

// Manages all rigidbody motion
public class RigidBodyManager : MonoBehaviour, IGameSystem
{
    public static RigidBodyManager instance;
    private List<RigidBody> rigidBodies = new List<RigidBody>(); // List of rigidbodies in the system
    public GameObject gravity; // Gravity vector
    public float maxVelocity; // Max velocity allowed by the system

    // Singleton pattern setup
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

    // Update all rigidbodies
    public void tick()
    {
        foreach (RigidBody rb in rigidBodies)
        {
            if (rb.gravity)
                rb.acceleration += gravity.transform.position;
            rb.UpdateRigidBody(Time.fixedDeltaTime);
        }
    }

    // Add a rigidbody to the system
    public void AddRigidBody(RigidBody rb)
    {
        if (!rigidBodies.Contains(rb))
        {
            rigidBodies.Add(rb);
        }
    }

    // Remove a rigidbody from the system
    public void RemoveRigidBody(RigidBody rb)
    {
        if (rigidBodies.Contains(rb))
        {
            rigidBodies.Remove(rb);
        }
    }
}
