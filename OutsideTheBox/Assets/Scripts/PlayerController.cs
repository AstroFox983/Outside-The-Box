using UnityEditor.ShaderGraph;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float rollForce = 15f;
    [SerializeField] float maxAngularVelocity = 20f;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float airControl = 0.5f; // How much control you have in the air (0 to 1)

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 7f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.6f; // Slightly more than half the cube size

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        // hide mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
    }

    void Update()
    {
        // Jump input goes in Update for better responsiveness
        if (Input.GetButtonDown("Jump") && CheckGrounded())
        {
            Jump();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void ApplyMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;

        Vector3 moveDirection = (forward.normalized * moveVertical + right.normalized * moveHorizontal).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            // 1. ROTATION (Visual Roll)
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, moveDirection);
            rb.AddTorque(rotationAxis * rollForce, ForceMode.Acceleration);

            // 2. DIRECTIONAL FORCE (Actual Momentum)
            float currentForce = isGrounded ? rollForce : rollForce * airControl;
            rb.AddForce(moveDirection * currentForce, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        // Ensure we keep our current horizontal velocity but "pop" upward
        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.y = jumpForce;
        rb.linearVelocity = currentVelocity;

        // Add a slight forward "nudge" if the player is holding a direction
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 forwardDir = (cameraTransform.forward * moveVertical + cameraTransform.right * moveHorizontal);
        forwardDir.y = 0;

        rb.AddForce(forwardDir.normalized * (jumpForce * 0.2f), ForceMode.VelocityChange);
    }

    bool CheckGrounded()
    {
        // SphereCast checks a circular area under the cube, making it more reliable
        return Physics.SphereCast(transform.position, 0.4f, Vector3.down, out _, 0.2f, groundLayer);
    }
}