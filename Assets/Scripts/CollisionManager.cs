using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionManager : MonoBehaviour, IGameSystem
{
    public static CollisionManager instance;

    public List<ICollider> colliders = new List<ICollider>();
    public List<Collision> collisions = new List<Collision>();

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
                if (pinball.GetComponent<ICollider>() != col && pinball.GetComponent<ICollider>().Intersects(col, out Collision collision))
                {
                    collisions.Add(collision);
                }
            }
        }

        foreach (Collision collision in collisions)
        {
            if (!collision.obj.GetComponent<ICollider>().isStatic)
            {
                try
                {
                    collision.obj.GetComponent<RigidBody>().AddImpulse(collision.normal * collision.obj.GetComponent<RigidBody>().mass);
                }
                catch
                {
                    throw new System.Exception("No RigidBody attached to non-static object");
                }
            }
            collision.obj.GetComponent<ICollider>().OnCollision(collision);
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
