using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 100f;
    [SerializeField]
    private GameObject leftBumper;
    private float leftBumperRotation;
    [SerializeField]
    private GameObject rightBumper;
    private float rightBumperRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftBumperRotation = leftBumper.transform.localEulerAngles.y;
        rightBumperRotation = rightBumper.transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Update left bumper position
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            leftBumper.transform.Rotate(new Vector3(0, -rotationSpeed, 0) * Time.deltaTime);
        else
            leftBumper.transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);
        leftBumper.transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(leftBumper.transform.localEulerAngles.y, leftBumperRotation - 45, leftBumperRotation), 0);

        // Update right bumper position
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            rightBumper.transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);
        else
            rightBumper.transform.Rotate(new Vector3(0, -rotationSpeed, 0) * Time.deltaTime);
        rightBumper.transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(rightBumper.transform.localEulerAngles.y, rightBumperRotation, rightBumperRotation + 45), 0);

        // Spawn pinball
        if (Input.GetKeyDown(KeyCode.Q))
            GameManager.instance.TrySpawnPinball();
    }

}
