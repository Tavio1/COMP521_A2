using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<GameObject> pinballs = new List<GameObject>();
    public int pinballCount = 0;
    public GameObject pinballPrefab;
    public GameObject pinballSpawnPoint;

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

    public void TrySpawnPinball()
    {
        if (pinballCount < 10)
        { 
            pinballs.Add(Instantiate(pinballPrefab, pinballSpawnPoint.transform.position, Quaternion.identity));
            pinballCount++;
        }
    }
}
