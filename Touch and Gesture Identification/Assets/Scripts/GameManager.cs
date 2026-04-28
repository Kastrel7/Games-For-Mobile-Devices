using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class GameManager : MonoBehaviour
{
    public GestureManager gestureManager;
    public Transform pivot;

    public Material defaultMaterial;
    public Material selectedMaterial;

    private IInteractable selectedObject;
    private GameObject selectedGameObject;

    public float cameraPanSpeed = 0.1f;
    public float fovSpeed = 0.1f;
    public int minFov = 40;
    public int maxFov = 80;
    public float orbitSpeed = 0.5f;

    private Vector3 camOriginalPosition;
    private Quaternion camOriginalRotation;
    private float camOriginalFOV;

    void Start()
    {
        gestureManager.OnTap += HandleTap;
        gestureManager.OnDrag += HandleDrag;
        gestureManager.OnPinch += HandlePinch;
        gestureManager.OnRotate += HandleRotate;
        gestureManager.OnThreeFingerTouch += HandleReset;
        gestureManager.OnShake += HandleReset;

        camOriginalPosition = Camera.main.transform.position;
        camOriginalRotation = Camera.main.transform.rotation;
        camOriginalFOV = Camera.main.fieldOfView;
    }

    private void HandleTap(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IInteractable tapped = hit.collider.GetComponent<IInteractable>();

            if (tapped != null)
            {
                if (tapped == selectedObject)
                    return;

                DeselectCurrent();
                SelectObject(tapped, hit.collider.gameObject);
                return;
            }
        }
        DeselectCurrent();
    }

    void HandleDrag(Vector2 delta)
    {
        if (selectedObject != null)
            selectedObject.OnDrag(delta);
        else
            OnCameraPan(delta);
    }

    void HandlePinch(float delta)
    {
        if (selectedObject != null)
            selectedObject.OnPinch(delta);
        else
            OnCameraZoom(delta);
    }

    void HandleRotate(float delta)
    {
        if (selectedObject != null)
            selectedObject.OnRotate(delta);
        else
            OnCameraOrbit(delta);
    }

    public void HandleReset()
    {
        MonoBehaviour[] allObjects = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour mb in allObjects)
        {
            if (mb is IInteractable i)
                i.OnReset();
        }
        Camera.main.transform.position = camOriginalPosition;
        Camera.main.transform.rotation = camOriginalRotation;
        Camera.main.fieldOfView = camOriginalFOV;
    }

    private void OnCameraPan(Vector2 delta)
    {
        Camera.main.transform.position += new Vector3(-delta.x, -delta.y, 0) * cameraPanSpeed * Time.deltaTime;
    }

    private void OnCameraZoom(float delta)
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - delta * fovSpeed, minFov, maxFov);
    }

    private void OnCameraOrbit(float delta)
    {
        Camera.main.transform.RotateAround(pivot.position, Vector3.up, -delta * orbitSpeed);
    }

    private void SelectObject(IInteractable interactable, GameObject go)
    {
        selectedObject = interactable;
        selectedGameObject = go;

        Renderer renderer = selectedGameObject.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material = selectedMaterial;

        selectedObject.OnSelect();
    }

    private void DeselectCurrent()
    {
        if (selectedObject == null || selectedGameObject == null)
            return;

        Renderer renderer = selectedGameObject.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material = defaultMaterial;

        selectedObject.OnDeselect();
        selectedObject = null;
        selectedGameObject = null;
    }
}
