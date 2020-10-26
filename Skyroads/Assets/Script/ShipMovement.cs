using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private float minX, maxX;

    [SerializeField] private float zoomSpeed = 3f;

    private readonly float boostCameraDistance = 2.12f;
    private readonly float boostCameraHeight = 1.07f;

    private SmoothFollow cameraFollow;
    private readonly float defaultCameraDistance = 6.43f;
    private readonly float defaultCameraHeight = 2.23f;

    private bool moving;
    private Rigidbody rb;

    private TileSpawner tileSpawner;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraFollow = FindObjectOfType<SmoothFollow>();
        tileSpawner = FindObjectOfType<TileSpawner>();
    }

    private void Update()
    {
        //set boundaries for ship position
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            transform.position.y,
            transform.position.z);
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            //move and rotate ship
            var moveX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector3(moveX, rb.velocity.y, rb.velocity.z) * speed * Time.deltaTime;
            //if no input, rotate back to zero
            rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(0, 0, -moveX * 60),
                Time.deltaTime * rotationSpeed);

            if (Input.GetKey(KeyCode.Space))
            {
                //zoom camera on space
                cameraFollow.distance =
                    Mathf.Lerp(cameraFollow.distance, boostCameraDistance, Time.deltaTime * zoomSpeed);
                cameraFollow.height = Mathf.Lerp(cameraFollow.height, boostCameraHeight, Time.deltaTime * zoomSpeed);
                //increase tiles speed while space pressed
                tileSpawner.BoostTileSpeed();
            }
            else
            {
                //zoom out the camera 
                cameraFollow.distance =
                    Mathf.Lerp(cameraFollow.distance, defaultCameraDistance, Time.deltaTime * zoomSpeed);
                cameraFollow.height = Mathf.Lerp(cameraFollow.height, defaultCameraHeight, Time.deltaTime * zoomSpeed);
                //decrease tiles speed back to normal
                tileSpawner.RemoveBoostSpeed();
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if player collides obstacle, notify that game ended
        if (other.gameObject.tag == "Obstacle")
        {
            moving = false;
            //fire loose event
            PlayerEvents.FirePlayerDeath();
        }
    }

    public void Move(bool isMoving)
    {
        moving = isMoving;
    }
}