using UnityEngine;

// Interface to represent different types of colliders
public abstract class ICollider : MonoBehaviour
{
    [SerializeField]
    public bool isStatic = false; // Does the object move?
    [Range(0f, 2f)]
    public float restitution = 1f; // Coeff of restitution

    // Add all colliders to the collision system
    virtual public void Start()
    {
        CollisionManager.instance.AddCollider(this);
    }
}
