using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum DragType { XZ, XY, CameraRay }
    public DragType dragType;

    private Renderer objectRenderer;
    private Material defaultMaterial;
    private float distanceToCamera;

    [SerializeField] private float dragSpeed = 0.01f;
    [SerializeField] private float scaleSpeed = 0.01f;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        defaultMaterial = objectRenderer.material;
    }

    public void OnSelect(Material selected)
    {
        objectRenderer.material = selected;
        distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
    }

    public void OnDeselect()
    {
        objectRenderer.material = defaultMaterial;
    }

    public void OnDrag(Vector2 delta, Vector2 screenPosition)
    {
        switch (dragType)
        {
            case DragType.XZ:
                transform.position += new Vector3(delta.x, 0, delta.y) * dragSpeed;
                break;

            case DragType.XY:
                transform.position += new Vector3(delta.x, delta.y, 0) * dragSpeed;
                break;

            case DragType.CameraRay:
                Ray ray = Camera.main.ScreenPointToRay(screenPosition);
                transform.position = ray.origin + ray.direction * distanceToCamera;
                break;
        }
    }

    public void OnPinch(float delta)
    {
        float scaleDelta = delta * scaleSpeed;
        transform.localScale += Vector3.one * scaleDelta;
        transform.position += new Vector3(0, scaleDelta / 2f, 0);
    }
}