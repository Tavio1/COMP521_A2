using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public abstract class ICollider : MonoBehaviour
{
    [SerializeField]
    public bool isStatic = false;
    [SerializeField]
    public bool isTrigger = false;

    void Start()
    {
        CollisionManager.instance.AddCollider(this);
    }

    public bool Intersects(ICollider other, out Collision collision)
    {
        collision = null;

        switch (other)
        {
            case CircleCollider circle:
                // Handle CircleCollider specific logic
                collision = IntersectsWithCircle(circle);
                break;
            // Add cases for other collider types as needed
            default:
                Debug.Log("Unknown collider type");
                break;
        }

        return collision != null;
    }

    public abstract Collision IntersectsWithCircle(CircleCollider other);
    public void OnCollision(Collision collision) { }
    //public abstract bool IntersectsWithSquare(SquareCollider other);
    //public abstract bool IntersectsWithTriangle(TriangleCollider other);


}
