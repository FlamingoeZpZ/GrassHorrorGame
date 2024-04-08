using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 moveDirection;

    private Rigidbody rb;
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private float maxSpeed = 6;
    [SerializeField] private float yawSpeed;
    [SerializeField] private float pitchSpeed;
    [SerializeField] private LayerMask houseLayer;
    [SerializeField] private GameObject FillUpUI;

    private Buckets bs;
    private GPT_ImplGrassPainter gen;
        
    // Start is called before the first frame update
    void Awake()
    {
        PlayerControls.Init(this);
        rb = transform.parent.GetComponent<Rigidbody>();
        bs = GetComponentInChildren<Buckets>();
        gen = GetComponent<GPT_ImplGrassPainter>();
        rb.maxLinearVelocity = maxSpeed;
        gen.SetBucket(bs);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
        var transform1 = transform;
        FillUpUI.SetActive(Physics.Raycast(transform1.position, transform1.forward ,2, houseLayer));
    }

    void HandleMovement()
    {
        //Simple translation Movement
        rb.AddForce(transform.rotation * (moveDirection * moveSpeed));
    }

    public void SetMoveDirection(Vector3 readValue)
    {
        moveDirection = readValue;
    }

    public void SetWateringState(bool readValueAsButton)
    {
        if(readValueAsButton) bs.StartWatering();
        else bs.StopWatering();
    }


    public float minPitchAngle = -89f; // Minimum pitch angle (looking down)
    public float maxPitchAngle = 89f;  // Maximum pitch angle (looking up)

    public void MoveCamera(Vector2 readValue)
    {
        float dt = Time.deltaTime;

        // Calculate the target pitch and yaw rotations
        Quaternion currentRotation = transform.rotation;
        Quaternion yawRotation = Quaternion.AngleAxis(readValue.x * yawSpeed * dt, Vector3.up);
        Quaternion pitchRotation = Quaternion.AngleAxis(-readValue.y * pitchSpeed * dt, Vector3.right);

        // Apply the rotations
        Quaternion newRotation = currentRotation * yawRotation * pitchRotation;

        // Get the pitch angle from the new rotation
        float pitchAngle = QuaternionToPitchAngle(newRotation);

        // Clamp the pitch angle to prevent looking too far up or down
        float clampedPitchAngle = Mathf.Clamp(pitchAngle, minPitchAngle, maxPitchAngle);

        // Calculate the new rotation with clamped pitch angle
        Quaternion finalRotation = Quaternion.Euler(clampedPitchAngle, newRotation.eulerAngles.y, 0);

        // Apply the final rotation
        transform.rotation = finalRotation;
    }

    private float QuaternionToPitchAngle(Quaternion q)
    {
        float sinPitch = 2 * (q.w * q.x - q.y * q.z);
        return Mathf.Asin(Mathf.Clamp(sinPitch, -1, 1)) * Mathf.Rad2Deg;
    }


    public void FillBucket()
    {
        if (Physics.Raycast(transform.position, transform.forward, 2, houseLayer))
        {
            bs.FillBucket();
        }
    }
}
