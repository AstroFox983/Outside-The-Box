using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] float sensitivity = 2f;
    float mouseX, mouseY;

    private void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
        mouseY = Mathf.Clamp(mouseY, -35f, 60f); // prevent camera flipping

        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
    }
}
