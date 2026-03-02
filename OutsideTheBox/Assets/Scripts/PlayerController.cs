using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;

    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private CinemachineVirtualCamera vCam;

    private Rigidbody rb;
    private float xRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleRotation();

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate direction relative to the player's current forward
        Vector3 moveDir = (transform.forward * moveZ) + (transform.right * moveX);

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f); 
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, 0.4f, Vector3.down, out _, 0.7f);
    }
}