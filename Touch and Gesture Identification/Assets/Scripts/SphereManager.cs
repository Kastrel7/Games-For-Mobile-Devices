using UnityEngine;

public class SphereManager : MonoBehaviour, IInteractable
{
    public float orbitSpeed = 0.1f;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Quaternion originalRotation;

    private Vector3 orbitCenter;

    void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
    }

    public void OnSelect()
    {
        orbitCenter = Camera.main.transform.position;
    }

    public void OnDeselect() { }

    public void OnDrag(Vector2 delta)
    {
        float horizontal = delta.x * orbitSpeed;
        float vertical = delta.y * orbitSpeed;

        transform.RotateAround(orbitCenter, Vector3.up, horizontal);
        transform.RotateAround(orbitCenter, Camera.main.transform.right, -vertical);
    }

    public void OnPinch(float delta)
    {
        Vector3 newScale = transform.localScale + Vector3.one * delta * 0.01f;
        newScale = Vector3.Max(newScale, Vector3.one * 0.1f);
        transform.localScale = newScale;
    }

    public void OnRotate(float delta)
    { 
        transform.Rotate(Vector3.up, delta, Space.World);
    }

    public void OnReset()
    {
        transform.position = originalPosition;
        transform.localScale = originalScale;
        transform.rotation = originalRotation;
    }
}
