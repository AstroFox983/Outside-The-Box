using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float rollForce = 10f;
    [SerializeField] float maxAngularVelocity = 15f;
    [SerializeField] float frictonDamping = 0.95f;
    [SerializeField] Transform cameraTransform;
    private Rigidbody rb;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 20f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // calculate direction relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // keep movement on the horizontal plane
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward* moveVertical + right * moveHorizontal).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, moveDirection);
            rb.AddTorque(rotationAxis * rollForce, ForceMode.Acceleration);
        }
        else
        {
            // apply brakes when no input
            rb.angularVelocity *= frictonDamping;
        }

        // hard limit to prevent excessive spinning
        if (rb.angularVelocity.magnitude > maxAngularVelocity)
        {
            rb.angularVelocity = rb.angularVelocity.normalized * maxAngularVelocity;
        }
    }
}