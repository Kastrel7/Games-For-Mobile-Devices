using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    private IInteractable currentInteractable;

    public float dragThreshold = 5f;
    public float dominantGestureThreshold = 5f;
    public float shakeThreshold = 2.5f;
    public float shakeCooldown = 1f;

    public event Action<Vector2> OnTap;
    public event Action<Vector2> OnDrag;
    public event Action OnDragEnd;

    public event Action<float> OnPinch;
    public event Action<float> OnRotate;

    public event Action OnThreeFingerTouch;

    public event Action OnShake;

    private bool isDragging = false;
    private Vector2 touchStartPos;

    private float previousPinchDistance;
    private Vector2 previousMidpoint;
    private Vector2 twoFingerStartMidpoint;

    private enum TwoFingerGesture { Undecided, Pinch, Rotate}
    private TwoFingerGesture activeTwoFingerGesture = TwoFingerGesture.Undecided;

    private float accumulatedPinchDelta;
    private float accumulatedRotationDelta;

    private float lastShakeTime;

    private void Update()
    {
        HandleShake();

        if (Input.touchCount == 1)
            HandleSingleTouch();
        else if (Input.touchCount == 2)
            HandleTwoFingerTouch();
        else if (Input.touchCount == 3)
            HandleThreeFingerTouch();
    }

    private void HandleSingleTouch()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            isDragging = false;
            touchStartPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            float distanceMoved = Vector2.Distance(touch.position, touchStartPos);

            if (!isDragging && distanceMoved > dragThreshold)
                isDragging = true;

            if (isDragging)
            {
                OnDrag?.Invoke(touch.deltaPosition);
            }
        }
        else if (touch.phase != TouchPhase.Ended)
        {
            float distanceMoved = Vector2.Distance(touch.position, touchStartPos);

            if (distanceMoved < dragThreshold)
                OnTap?.Invoke(touch.position);
            else
                OnDragEnd?.Invoke();

            isDragging = false;
        }
    }

    private void HandleTwoFingerTouch()
    {
        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
        {
            previousPinchDistance = Vector2.Distance(t0.position, t1.position);

            twoFingerStartMidpoint = (t0.position + t1.position) / 2f;
            previousMidpoint = twoFingerStartMidpoint;

            activeTwoFingerGesture = TwoFingerGesture.Undecided;
            accumulatedPinchDelta = 0f;
            accumulatedRotationDelta = 0f;
        }
        else if (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)
        {
            float currentPinchDistance = Vector2.Distance(t0.position, t1.position);
            float pinchDelta = currentPinchDistance - previousPinchDistance;

            Vector2 currentMidpoint = (t0.position + t1.position) / 2f;
            Vector2 swipeDelta = currentMidpoint - twoFingerStartMidpoint;

            if (activeTwoFingerGesture == TwoFingerGesture.Undecided)
            {
                accumulatedPinchDelta += Mathf.Abs(pinchDelta);
                accumulatedRotationDelta += swipeDelta.magnitude;

                if (accumulatedPinchDelta > dominantGestureThreshold)
                    activeTwoFingerGesture = TwoFingerGesture.Pinch;
                else if (accumulatedRotationDelta > dominantGestureThreshold)
                    activeTwoFingerGesture = TwoFingerGesture.Rotate;
            }

            if (activeTwoFingerGesture == TwoFingerGesture.Pinch)
            {
                if (Mathf.Abs(pinchDelta) > 0.01f)
                    OnPinch?.Invoke(pinchDelta);
            }
            else if (activeTwoFingerGesture == TwoFingerGesture.Rotate)
            {
                Vector2 frameDelta = currentMidpoint - previousMidpoint;
                if (frameDelta.magnitude > 0.01f)
                    OnRotate?.Invoke(swipeDelta.x * 0.1f);
            }

            previousPinchDistance = currentPinchDistance;
            previousMidpoint = currentMidpoint;
        }
        else if (t0.phase == TouchPhase.Ended || t1.phase == TouchPhase.Ended)
        {
            activeTwoFingerGesture = TwoFingerGesture.Undecided;
            accumulatedPinchDelta = 0f;
            accumulatedRotationDelta = 0f;
        }
    }

    private void HandleThreeFingerTouch()
    {
        Touch t0 = Input.GetTouch(0);

        if (t0.phase == TouchPhase.Began)
            OnThreeFingerTouch?.Invoke();
    }

    private void HandleShake()
    {
        Vector3 acceleration = Input.acceleration;
        Vector3 linearAcceleration = acceleration - new Vector3(0, -1f, 0);

        if (linearAcceleration.magnitude > shakeThreshold)
        {
            if (Time.time - lastShakeTime > shakeCooldown)
            {
                lastShakeTime = Time.time;
                OnShake?.Invoke();
            }
        }
    }
}