using UnityEngine;

public interface IInteractable
{
    void OnSelect();
    void OnDeselect();
    void OnDrag(Vector2 delta);
    void OnPinch(float delta);
    void OnRotate(float delta);
    void OnReset();
}
