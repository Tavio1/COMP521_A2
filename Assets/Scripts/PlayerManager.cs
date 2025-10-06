using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameSystem
{
    public static PlayerManager instance;
    [SerializeField]
    private float rotationSpeed = 100f;
    [SerializeField]
    private GameObject leftBumper;
    private float leftBumperRotation;
    [SerializeField]
    private GameObject rightBumper;
    private float rightBumperRotation;

    private bool leftBumperActive;
    private bool rightBumperActive;
    private bool isShaking;

    public void tick()
    {
        // Update left bumper position
        if (leftBumperActive)
            leftBumper.transform.Rotate(new Vector3(0, -rotationSpeed, 0) * Time.fixedDeltaTime);
        else
            leftBumper.transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.fixedDeltaTime);
        leftBumper.transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(leftBumper.transform.localEulerAngles.y, leftBumperRotation - 45, leftBumperRotation), 0);

        // Update right bumper position
        if (rightBumperActive)
            rightBumper.transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.fixedDeltaTime);
        else
            rightBumper.transform.Rotate(new Vector3(0, -rotationSpeed, 0) * Time.fixedDeltaTime);
        rightBumper.transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(rightBumper.transform.localEulerAngles.y, rightBumperRotation, rightBumperRotation + 45), 0);

        // Add shake
        if (isShaking)
            foreach (GameObject pinball in GameManager.instance.pinballs)
            {
                RigidBody rb = pinball.GetComponent<RigidBody>();
                rb.AddImpulse(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)));
            }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftBumperRotation = leftBumper.transform.localEulerAngles.y;
        rightBumperRotation = rightBumper.transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        leftBumperActive = Input.GetKey(KeyCode.A);
        rightBumperActive = Input.GetKey(KeyCode.D);
        isShaking = Input.GetKey(KeyCode.Z);

        // Spawn pinball
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.instance.TrySpawnPinball();

        if (Input.GetKeyDown(KeyCode.Q))
            foreach (GameObject pinball in GameManager.instance.pinballs)
                GameManager.instance.DeleteObject(pinball);
    }

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

}
