using System.Collections.Generic;
using UnityEngine;

// Represents the rigidbody physics of an object
public class RigidBody : MonoBehaviour
{
    public bool gravity = false; // is it affected by gravity?
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

        // Add all rigidbodies to the rigidbody system
        RigidBodyManager.instance.AddRigidBody(this);
    }

    // Update the rigidbody from the last frame
    public void UpdateRigidBody(float deltaTime)
    {
        // Store the previous position
        lastPos = transform.position;

        // Update velocity based on acceleration
        velocity += acceleration * deltaTime;

        // Add all accumulated impulses from the last frame
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

    // Add impulse force
    public void AddImpulse(Vector3 impulse)
    {
        accumulatedImpulses.Add(impulse);
    }

    // Draw the velocity of the rigidbody    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + velocity);
    }
}
