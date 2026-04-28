using UnityEngine;

public class CapsuleManager : MonoBehaviour, IInteractable
{
    public float dragSpeed = 0.02f;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Quaternion originalRotation;

    private float distanceFromCamera;
    private Vector3 screenOffset;

    void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
    }

    public void OnSelect()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenOffset = screenPos - new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0f);
        distanceFromCamera = screenPos.z;
    }

    public void OnDeselect()
    {

    }

    public void OnDrag(Vector2 delta)
    {
        Vector2 touchPos = Input.GetTouch(0).position;
        Vector3 targetScreenPos = new Vector3(touchPos.x + screenOffset.x, touchPos.y + screenOffset.y, distanceFromCamera);
        transform.position = Camera.main.ScreenToWorldPoint(targetScreenPos);
    }

    public void OnPinch(float delta)
    {
        Vector3 newScale = transform.localScale + Vector3.one * delta * 0.01f;
        newScale = Vector3.Max(newScale, Vector3.one * 0.1f);
        transform.localScale = newScale;
    }

    public void OnRotate(float delta)
    {
        transform.Rotate(Vector3.forward, delta, Space.World);
    }

    public void OnReset()
    {
        transform.position = originalPosition;
        transform.localScale = originalScale;
        transform.rotation = originalRotation;
    }
}
