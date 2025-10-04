using UnityEngine;

public class RigidBody : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 acceleration;
    public float mass = 1.0f;

    public Vector3 lastPos;

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

        // Update position based on velocity
        transform.position += velocity * deltaTime;

        // Reset acceleration for the next frame
        acceleration = Vector3.zero;

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
        Vector3 impulseVelocityChange = impulse / mass;
        velocity += impulseVelocityChange;
    }
}
