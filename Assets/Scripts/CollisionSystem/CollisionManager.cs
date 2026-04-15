using System;
using System.Collections.Generic;
using UnityEngine;

// Manages all of the colliders and collisions
public class CollisionManager : MonoBehaviour, IGameSystem
{
    public static CollisionManager instance;

    public List<ICollider> colliders = new List<ICollider>(); // Colliders to tick
    public List<Collision> collisions = new List<Collision>(); // Collision events to resolve

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

    // Ticks the collision system, detecting new collisions and resolving them
    public void tick()
    {
        // Detect collisions b/w pinballs and other objects
        foreach (GameObject pinball in GameManager.instance.pinballs)
        {
            foreach (ICollider col in colliders)
            {
                if (pinball.GetComponent<ICollider>() == col) continue;

                if (pinball.GetComponent<PinballCollider>().Intersects(col, out Collision collision))
                {
                    collisions.Add(collision);
                }
            }
        }

        // Resolve all collision events created
        foreach (Collision collision in collisions)
        {
            // Destroy pinball if collides with hazard
            if (collision.other.CompareTag("DestroyPinball") && collision.obj.CompareTag("Pinball"))
            {
                GameManager.instance.DeleteObject(collision.obj);
                continue;
            }

            // Collision between pinball and static object
            if (collision.other.GetComponent<ICollider>().isStatic)
            {
                // Calculate the impulse force
                RigidBody rb = collision.obj.GetComponent<RigidBody>();
                Vector3 vNeg = rb.velocity;
                Vector3 vNegNorm = Vector3.Dot(vNeg, collision.normal) * collision.normal;
                float restitution = collision.other.GetComponent<ICollider>().restitution;
                float j = (1f + restitution) * collision.obj.GetComponent<RigidBody>().mass * vNegNorm.magnitude;
                Vector3 impulse = j * collision.normal;

                if (restitution > 1) UIManager.instance.AddPoints(250);
                else if (restitution < 0.5) UIManager.instance.AddPoints(100);

                // Add the impulse force
                rb.AddImpulse(impulse);
            }
            // Collision between pinballs
            else if (collision.other.CompareTag("Pinball"))
            {
                // Calculate the impulse force
                RigidBody rb1 = collision.obj.GetComponent<RigidBody>();
                RigidBody rb2 = collision.other.GetComponent<RigidBody>();

                Vector3 v1 = rb1.velocity;
                Vector3 v2 = rb2.velocity;

                Vector3 v1Prime = v1 - (Vector3.Dot(v1 - v2, collision.normal) * 2 * rb2.mass / (rb1.mass + rb2.mass)) * collision.normal;

                // Add the impulse force
                rb1.AddImpulse(v1Prime);
            }
            // Collision between pinball and bumper
            else
            {
                // Calculate the impulse force
                RigidBody rb = collision.obj.GetComponent<RigidBody>();
                RigidBody bRb = collision.other.GetComponentInParent<RigidBody>();

                Vector3 vNeg = rb.velocity;
                Vector3 vNegNorm = Vector3.Dot(vNeg, collision.normal) * collision.normal;
                float j = (1f + collision.other.GetComponent<ICollider>().restitution) * collision.obj.GetComponent<RigidBody>().mass * vNegNorm.magnitude;
                Vector3 impulse = j * collision.normal;

                // Add the impulse force
                rb.AddImpulse(impulse);

                // If bumper is moving, move the pinball out to avoid getting stuck within the bumper
                if (!bRb.lastPos.Equals(bRb.transform.position))
                    collision.obj.transform.position = collision.point + collision.normal * 0.1f;
            }
        }

        // Clear the collision events for this tick
        collisions.Clear();
    }

    // Adds a collider to the system
    public void AddCollider(ICollider collider)
    {
        if (!colliders.Contains(collider))
        {
            colliders.Add(collider);
        }
    }

    // Removes a collider from the system
    public void RemoveCollider(ICollider collider)
    {
        if (colliders.Contains(collider))
        {
            colliders.Remove(collider);
        }
    }
}
