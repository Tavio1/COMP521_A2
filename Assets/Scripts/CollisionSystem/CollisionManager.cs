using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        // // Detect collisions
        // foreach (ICollider colA in colliders)
        // {
        //     foreach (ICollider colB in colliders)
        //     {
        //         if (colA != colB && colA.Intersects(colB, out Collision collision))
        //         {
        //             collisions.Add(collision);
        //             // colA.OnCollision(collision);
        //             // colB.OnCollision(collision);
        //         }
        //     }
        // }
        foreach (GameObject pinball in GameManager.instance.pinballs)
        {
            foreach (ICollider col in colliders)
            {
                if (pinball.GetComponent<ICollider>() != col && pinball.GetComponent<PinballCollider>().Intersects(col, out Collision collision))
                {
                    collisions.Add(collision);
                }
            }
        }

        foreach (Collision collision in collisions)
        {
            if (!collision.obj.GetComponent<ICollider>().isStatic)
            {
                if (collision.other.CompareTag("DestroyPinball") && collision.obj.CompareTag("Pinball"))
                {
                    GameManager.instance.DeleteObject(collision.obj);
                    continue;
                }
                try
                {
                    Vector3 vNeg = collision.obj.transform.position - collision.point;
                    Vector3 vNegNorm = Vector3.Dot(vNeg, collision.normal) * collision.normal;
                    Vector3 vPosNorm = -vNegNorm;
                    float j = (1f + collision.other.GetComponent<ICollider>().restitution) * collision.obj.GetComponent<RigidBody>().mass * vNegNorm.magnitude;
                    //Vector3 impulse = j * collision.normal / Time.fixedDeltaTime; // Avoid div by zero
                    Vector3 impulse = j * collision.normal / Mathf.Max((Time.fixedDeltaTime - collision.time), 0.0001f); // Avoid div by zero
                    collision.obj.GetComponent<RigidBody>().AddImpulse(impulse);
                    collision.obj.transform.position = collision.point;


                    //Debug.Log("Collision at " +  collision.point + " with normal: " + collision.normal + ", j: " + j + ", impulse: " + impulse );
                }
                catch
                {
                    throw new System.Exception("No RigidBody attached to non-static object");
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
