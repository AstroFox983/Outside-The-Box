using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float sinkAmount = 0.1f;
    [SerializeField] float sinkSpeed = 5f;

    [Header("Events")]
    [SerializeField] UnityEvent OnActivated;

    private bool isPressed = false;
    private Vector3 initialLocalPos;
    private Vector3 targetLocalPos;

    private void Start()
    {
        initialLocalPos = transform.localPosition;
        targetLocalPos = initialLocalPos; ;
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPos, Time.deltaTime * sinkSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Button Pressed!");
            ActivateButton();
        }
    }

    void ActivateButton()
    {
        isPressed = true;

        // calculate target position for sinking
        targetLocalPos = initialLocalPos + new Vector3(0, -sinkAmount, 0);

        OnActivated.Invoke(); // trigger events to activate connected objects
    }
}
