using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour, IGameSystem
{
    public static CollisionManager instance;

    public List<ICollider> colliders = new List<ICollider>();
    public List<Collision> collisions = new List<Collision>();
    public List<Collision> debugCollisions = new List<Collision>();
 
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
                RigidBody rb = collision.obj.GetComponent<RigidBody>();
                Vector3 vNeg = rb.velocity;
                Vector3 vNegNorm = Vector3.Dot(vNeg, collision.normal) * collision.normal;
                float j = (1f + collision.other.GetComponent<ICollider>().restitution) * collision.obj.GetComponent<RigidBody>().mass * vNegNorm.magnitude;
                Vector3 impulse = j * collision.normal;
                collision.obj.GetComponent<RigidBody>().AddImpulse(impulse);
                collision.obj.transform.position = collision.point;
            }
            // Collision between pinballs
            else if (collision.other.CompareTag("Pinball"))
            {
                RigidBody rb1 = collision.obj.GetComponent<RigidBody>();
                RigidBody rb2 = collision.other.GetComponent<RigidBody>();

                Vector3 v1 = rb1.velocity;
                Vector3 v2 = rb2.velocity;

                Vector3 v1Prime = v1 - (Vector3.Dot(v1 - v2, collision.normal) * 2 * rb2.mass / (rb1.mass + rb2.mass)) * collision.normal;

                //Debug.Log(rb1.name + " with " + v1Prime);
                rb1.AddImpulse(v1Prime);
            }
            // Collision between pinball and bumper
            else
            {
                RigidBody rb = collision.obj.GetComponent<RigidBody>();
                RigidBody bRb = collision.other.GetComponentInParent<RigidBody>();

                Vector3 vNeg = rb.velocity;
                Vector3 vNegNorm = Vector3.Dot(vNeg, collision.normal) * collision.normal;
                float j = (1f + collision.other.GetComponent<ICollider>().restitution) * collision.obj.GetComponent<RigidBody>().mass * vNegNorm.magnitude;
                Vector3 impulse = j * collision.normal;
                collision.obj.GetComponent<RigidBody>().AddImpulse(impulse);
                collision.obj.transform.position = collision.point;

                if (bRb.angularVelocity != 0f)
                {
                    BoxCollider bCol = collision.other.GetComponent<BoxCollider>();
                    float boxLength = bCol.maxCorner.x - bCol.minCorner.x;
                    if (collision.normal.Equals(Vector3.zero))
                        collision.normal = bCol.transform.rotation * Vector3.back;

                    Vector3 perp = new Vector3(-collision.normal.z, 0, collision.normal.x).normalized;
                    Vector3 difference = collision.point - bRb.transform.position;
                    float distanceAlongPaddle = Mathf.Abs(Vector3.Dot(difference, perp)) / boxLength;

                    Vector3 impulse2 = distanceAlongPaddle * bRb.angularVelocity * collision.normal;
                    collision.obj.GetComponent<RigidBody>().AddImpulse(impulse2);
                    collision.obj.transform.position = collision.point;
                }

            }
        }

        collisions.Clear();
    }

    public void AddCollider(ICollider collider)
    {
        if (!colliders.Contains(collider))
        {
            colliders.Add(collider);
        }
    }

    public void RemoveCollider(ICollider collider)
    {
        if (colliders.Contains(collider))
        {
            colliders.Remove(collider);
        }
    }
}
