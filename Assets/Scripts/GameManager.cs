using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameSystem
{
    public static GameManager instance;
    [HideInInspector]
    public List<GameObject> pinballs = new List<GameObject>(); // List of all current pinballs
    [HideInInspector]
    public int pinballCount = 0; // Count of number of active pinballs
    public GameObject pinballPrefab;
    private List<GameObject> objectsToDestroy = new List<GameObject>(); // List of objects to destroy at the end of the frame
    public int ticks = 0; // Total count of frames
    private int pinballsSpawned = 0; // Total count of pinballs spawned (used for naming and debugging)

    [Header("Debug")]
    public bool spawnHazards = true;
    public bool fixedPinballSpawn = false;
    public GameObject pinballSpawnPoint;

    [Header("Hazard Spawning")]
    public GameObject hazardParent; // What object to set the hazards parents as
    private List<Vector3> spawnLocations = new List<Vector3>(); // List of locations hazards are spawned at

    public GameObject trianglePrefab;
    public GameObject circlePrefab;

    // Singleton pattern setup
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

    // Spawns hazards at random locations on the board
    void Start()
    {
        if (!spawnHazards) return;

        Vector3 selectedPos = Vector3.zero;
        bool posValid = false;

        // Spawn 7 hazards at random locations
        for (int i = 0; i < 7; i++)
        {
            // Generate random locations until we've found one far enough away from all others
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

            // Store this spawn location
            spawnLocations.Add(selectedPos);

            // Spawn obstacle at the selected location
            GameObject obstacle = i < 3 ? circlePrefab : trianglePrefab; // Spawn 3 circles and 4 triangles
            Quaternion rotation = Quaternion.Euler(new Vector3(0, Random.Range(10, 50), 0)); // Randomize the rotation to avoid flat edge on triangles
            Instantiate(obstacle, selectedPos, rotation, hazardParent.transform);
        }
    }

    // Generates a random spawn point for the hazards within a defined area
    private Vector3 GenerateHazardSpawn()
    {
        return new Vector3(Random.Range(2.5f, 11f), 0, Random.Range(7.5f, 25f));
    }

    // Generates a random spawn point for the pinball along the top of the board
    private Vector3 GeneratePinballSpawn()
    {
        return fixedPinballSpawn ? pinballSpawnPoint.transform.position :
                    new Vector3(0.5f + (Random.Range(0, 2) * 8 + (Random.Range(0f, 4))), 0, 28f);
    }

    // Tick all systems at a fixed interval and in the defined order
    void FixedUpdate()
    {
        PlayerManager.instance.tick();
        RigidBodyManager.instance.tick();
        CollisionManager.instance.tick();
        GameManager.instance.tick();
        ticks++;
    }

    // Public method to mark an object for deletion from other systems
    public void DeleteObject(GameObject obj)
    {
        objectsToDestroy.Add(obj);
    }

    // Removes any rigidbodies or colliders on obj from their respective systems and destroys the obejct
    private void RemoveObject(GameObject obj)
    {
        RigidBodyManager.instance.RemoveRigidBody(obj.GetComponent<RigidBody>());
        CollisionManager.instance.RemoveCollider(obj.GetComponent<ICollider>());

        Destroy(obj);
    }

    // Spawns a pinball if there is less than 2
    public void TrySpawnPinball()
    {
        if (pinballCount < 2)
        {
            pinballs.Add(Instantiate(pinballPrefab, GeneratePinballSpawn(), Quaternion.identity));
            pinballs[pinballs.Count - 1].name = pinballs[pinballs.Count - 1].name + " " + pinballsSpawned;
            pinballCount++;
            pinballsSpawned++;
        }
    }

    // Removes a pinball safely
    private void RemovePinball(GameObject pinball)
    {
        if (!pinball.CompareTag("Pinball"))
            return;

        pinballCount--;
        pinballs.Remove(pinball);
        RemoveObject(pinball);
    }

    // Destroys any objects marked for deletion
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
