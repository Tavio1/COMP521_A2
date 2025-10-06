using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameSystem
{
    public static GameManager instance;
    [HideInInspector]
    public List<GameObject> pinballs = new List<GameObject>();
    [HideInInspector]
    public int pinballCount = 0;
    public GameObject pinballPrefab;
    public GameObject pinballSpawnPoint;
    private List<GameObject> objectsToDestroy = new List<GameObject>();
    public int ticks = 0;
    private int pinballsSpawned = 0;

    [Header("Hazard Spawning")]
    public GameObject hazardParent;
    private List<Vector3> spawnLocations = new List<Vector3>();

    public GameObject trianglePrefab;
    public GameObject circlePrefab;

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

    void Start()
    {
        Vector3 selectedPos = Vector3.zero;
        bool posValid = false;

        for (int i = 0; i < 7; i++)
        {
            posValid = false;
            while (!posValid)
            {
                selectedPos = GenerateHazardSpawn();
                posValid = true;

                foreach (Vector3 otherPos in spawnLocations)
                {
                    if (Vector3.Distance(selectedPos, otherPos) <= 4.5f)
                        posValid = false;
                }
            }

            spawnLocations.Add(selectedPos);

            // Spawn obstacle at the selected location
            GameObject obstacle = i < 3 ? circlePrefab : trianglePrefab;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, Random.Range(10, 50), 0));
            Instantiate(obstacle, selectedPos, rotation, hazardParent.transform);
        }
    }

    private Vector3 GenerateHazardSpawn()
    {
        return new Vector3(Random.Range(2.5f, 11f), 0, Random.Range(7.5f, 25f));
    }

    private Vector3 GeneratePinballSpawn()
    {
        return new Vector3(0.5f + (Random.Range(0, 2) * 8 + (Random.Range(0f, 4))), 0, 28f);
    }

    void FixedUpdate()
    {
        PlayerManager.instance.tick();
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
            //Vector3 spawnLocation = pinballSpawnPoint.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
            pinballs.Add(Instantiate(pinballPrefab, GeneratePinballSpawn(), Quaternion.identity));
            pinballs[pinballs.Count - 1].name = pinballs[pinballs.Count - 1].name + " " + pinballsSpawned;
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
