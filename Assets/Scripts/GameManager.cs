using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameSystem
{
    public static GameManager instance;
    public List<GameObject> pinballs = new List<GameObject>();
    public int pinballCount = 0;
    public GameObject pinballPrefab;
    public GameObject pinballSpawnPoint;
    private List<GameObject> objectsToDestroy = new List<GameObject>();
    public int ticks = 0;
    private int pinballsSpawned = 0;

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
        GameManager.instance.tick();
        ticks++;
    }

    public void DeleteObject(GameObject obj)
    {
        objectsToDestroy.Add(obj);
    }

    private void RemoveObject(GameObject obj)
    {
        RigidBodyManager.instance.RemoveRigidBody(obj.GetComponent<RigidBody>());
        CollisionManager.instance.RemoveCollider(obj.GetComponent<ICollider>());

        Destroy(obj);
    }

    public void TrySpawnPinball()
    {
        if (pinballCount < 2)
        {
            Vector3 spawnLocation = pinballSpawnPoint.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
            pinballs.Add(Instantiate(pinballPrefab, spawnLocation, Quaternion.identity));
            pinballs[pinballs.Count-1].name = pinballs[pinballs.Count-1].name + " " + pinballsSpawned;
            pinballCount++;
            pinballsSpawned++;
        }
    }

    private void RemovePinball(GameObject pinball)
    {
        if (!pinball.CompareTag("Pinball"))
            return;

        pinballCount--;
        pinballs.Remove(pinball);
        RemoveObject(pinball);
    }

    public void tick()
    {
        for (int i = objectsToDestroy.Count - 1; i >= 0; i--)
        {
            GameObject destroy = objectsToDestroy[i];
            objectsToDestroy.RemoveAt(i);

            if (destroy.CompareTag("Pinball"))
                RemovePinball(destroy);
            else
                RemoveObject(destroy);
        }
    }
}
