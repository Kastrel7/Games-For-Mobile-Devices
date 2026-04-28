using UnityEngine;

public class CubeManager : MonoBehaviour, IInteractable
{
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Quaternion originalRotation;

    private Plane dragPlane;

    void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
    }

    public void OnSelect()
    {
        dragPlane = new Plane(Vector3.up, transform.position);
    }

    public void OnDeselect()
    {

    }

    public void OnDrag(Vector2 delta)
    {
        Vector2 currentScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 targetScreenPos = currentScreenPos + delta;

        Ray ray = Camera.main.ScreenPointToRay(targetScreenPos);

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPos = ray.GetPoint(enter);
            transform.position = worldPos;
        }
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
