using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public abstract class ICollider : MonoBehaviour
{
    [SerializeField]
    public bool isStatic = false;
    [Range(0f, 2f)]
    public float restitution = 1f;

    virtual public void Start()
    {
        CollisionManager.instance.AddCollider(this);
    }

    //public abstract bool IntersectsWithSquare(SquareCollider other);
    //public abstract bool IntersectsWithTriangle(TriangleCollider other);


}
