using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float sinkAmount = 0.1f;
    [SerializeField] float sinkSpeed = 5f;
    [SerializeField] LayerMask detectionLayer;

    [Header("Events")]
    [SerializeField] UnityEvent OnPress;
    [SerializeField] UnityEvent OnRelease;

    private int objectsOnButton;
    private Vector3 initialLocalPos;
    private Vector3 pressedLocalPos;
    private Vector3 targetLocalPos;

    private void Start()
    {
        initialLocalPos = transform.localPosition;
        pressedLocalPos = initialLocalPos + new Vector3(0, -sinkAmount, 0);
        targetLocalPos = initialLocalPos; ;
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPos, Time.deltaTime * sinkSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & detectionLayer) != 0)
        {
            objectsOnButton++;
            if (objectsOnButton == 1)
            {
                Press();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & detectionLayer) != 0)
        {
            objectsOnButton--;
            if (objectsOnButton <= 0)
            {
                objectsOnButton = 0;
                Release();
            }
        }
    }

    void Press()
    {
        targetLocalPos = pressedLocalPos;
        OnPress?.Invoke();
        Debug.Log("Button Pressed!");
    }

    void Release()
    {
        targetLocalPos = initialLocalPos;
        OnRelease?.Invoke();
        Debug.Log("Button Released!");
    }
}
