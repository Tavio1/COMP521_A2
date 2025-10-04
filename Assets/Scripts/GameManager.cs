using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    void FixedUpdate()
    {
        RigidBodyManager.instance.tick();
        CollisionManager.instance.tick();
    }

    void RemoveObject(GameObject obj)
    {
        RigidBodyManager.instance.RemoveRigidBody(obj.GetComponent<RigidBody>());
        CollisionManager.instance.RemoveCollider(obj.GetComponent<ICollider>());

        Destroy(obj);
    }
}
