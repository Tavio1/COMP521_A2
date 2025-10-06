using System.Collections.Generic;
using UnityEngine;

public class RigidBody : MonoBehaviour
{
    public bool gravity = false;
    public float mass = 1.0f;

    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public Vector3 acceleration;
    [HideInInspector]
    public Vector3 lastPos;

    private List<Vector3> accumulatedImpulses = new List<Vector3>();

    void Start()
    {
        // Initialize velocity and acceleration
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        lastPos = transform.position;

        RigidBodyManager.instance.AddRigidBody(this);
    }

    public void UpdateRigidBody(float deltaTime)
    {
        // Store the previous position
        lastPos = transform.position;

        // Update velocity based on acceleration
        velocity += acceleration * deltaTime;

        foreach (Vector3 impulse in accumulatedImpulses)
            velocity += impulse;

        // Clamp velocity
        velocity = Vector3.ClampMagnitude(velocity, RigidBodyManager.instance.maxVelocity);

        // Update position based on velocity
        transform.position += velocity * deltaTime;

        // Reset acceleration and remove accumulated impulses for next frame
        acceleration = Vector3.zero;
        accumulatedImpulses.Clear();

    }

    public void ApplyForce(Vector3 force)
    {
        // F = m * a => a = F / m
        Vector3 forceAcceleration = force / mass;
        acceleration += forceAcceleration;
    }

    public void AddImpulse(Vector3 impulse)
    {
        // Impulse changes velocity directly
        //Vector3 impulseVelocityChange = impulse;
        //velocity += impulseVelocityChange;
        accumulatedImpulses.Add(impulse);
    }

    void OnDrawGizmos()
    { 
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + velocity);
    }
}
