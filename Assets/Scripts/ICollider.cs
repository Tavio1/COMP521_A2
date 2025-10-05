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

    public void OnCollision(Collision collision) { }
    //public abstract bool IntersectsWithSquare(SquareCollider other);
    //public abstract bool IntersectsWithTriangle(TriangleCollider other);


}
