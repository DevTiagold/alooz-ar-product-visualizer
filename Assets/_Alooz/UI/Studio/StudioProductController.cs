using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch =
    UnityEngine.InputSystem.EnhancedTouch.Touch;

public class StudioProductController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform productRoot;

    [SerializeField]
    private Camera studioCamera;


    [Header("Rotation")]
    [SerializeField]
    private float rotationSpeed = 0.2f;


    [Header("Zoom")]
    [SerializeField]
    private float zoomSpeed = 0.005f;

    [SerializeField]
    private float minCameraDistance = 1.5f;

    [SerializeField]
    private float maxCameraDistance = 8f;


    [Header("Pan")]
    [SerializeField]
    private float panSpeed = 0.002f;


    // ROTATION
    private Vector2 lastTouchPosition;
    private bool isDragging;


    // TWO-FINGER GESTURE
    private bool twoFingerGestureActive;

    private float lastPinchDistance;

    private Vector2 lastFingerCenter;


    // INITIAL STATE
    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;

    private Vector3 initialProductPosition;
    private Quaternion initialProductRotation;


    private void Start()
    {
        SaveInitialView();
    }


    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }


    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }


    private void Update()
    {
        HandleInput();
    }


    // ========================================
    // INPUT
    // ========================================

    private void HandleInput()
    {
        int touchCount =
            Touch.activeTouches.Count;


        if (touchCount == 1)
        {
            twoFingerGestureActive = false;

            HandleRotation();
        }
        else if (touchCount == 2)
        {
            isDragging = false;

            HandleTwoFingerGesture();
        }
        else
        {
            isDragging = false;

            twoFingerGestureActive = false;
        }
    }


    // ========================================
    // ROTATION
    // ========================================

    private void HandleRotation()
    {
        if (productRoot == null)
            return;

        if (studioCamera == null)
            return;


        Touch touch =
            Touch.activeTouches[0];


        if (
            touch.phase ==
            UnityEngine.InputSystem.TouchPhase.Began
        )
        {
            lastTouchPosition =
                touch.screenPosition;

            isDragging = true;

            return;
        }


        if (
            touch.phase ==
            UnityEngine.InputSystem.TouchPhase.Ended ||
            touch.phase ==
            UnityEngine.InputSystem.TouchPhase.Canceled
        )
        {
            isDragging = false;

            return;
        }


        if (!isDragging)
            return;


        Vector2 currentPosition =
            touch.screenPosition;


        Vector2 delta =
            currentPosition -
            lastTouchPosition;


        lastTouchPosition =
            currentPosition;


        float horizontalAngle =
            -delta.x *
            rotationSpeed;


        float verticalAngle =
            delta.y *
            rotationSpeed;


        Vector3 cameraUp =
            studioCamera.transform.up;


        Vector3 cameraRight =
            studioCamera.transform.right;


        Quaternion horizontalRotation =
            Quaternion.AngleAxis(
                horizontalAngle,
                cameraUp
            );


        Quaternion verticalRotation =
            Quaternion.AngleAxis(
                verticalAngle,
                cameraRight
            );


        productRoot.rotation =
            horizontalRotation *
            verticalRotation *
            productRoot.rotation;
    }


    // ========================================
    // TWO FINGERS
    // ZOOM + PAN
    // ========================================

    private void HandleTwoFingerGesture()
    {
        if (productRoot == null)
            return;

        if (studioCamera == null)
            return;


        Touch touchA =
            Touch.activeTouches[0];

        Touch touchB =
            Touch.activeTouches[1];


        Vector2 positionA =
            touchA.screenPosition;

        Vector2 positionB =
            touchB.screenPosition;


        float currentDistance =
            Vector2.Distance(
                positionA,
                positionB
            );


        Vector2 currentCenter =
            (
                positionA +
                positionB
            ) * 0.5f;


        // Primeiro frame do gesto
        if (!twoFingerGestureActive)
        {
            lastPinchDistance =
                currentDistance;

            lastFingerCenter =
                currentCenter;

            twoFingerGestureActive =
                true;

            return;
        }


        // ----------------------------
        // ZOOM
        // ----------------------------

        float pinchDelta =
            currentDistance -
            lastPinchDistance;


        lastPinchDistance =
            currentDistance;


        ApplyZoom(
            pinchDelta
        );


        // ----------------------------
        // PAN
        // ----------------------------

        Vector2 centerDelta =
            currentCenter -
            lastFingerCenter;


        lastFingerCenter =
            currentCenter;


        ApplyPan(
            centerDelta
        );
    }


    // ========================================
    // ZOOM
    // ========================================

    private void ApplyZoom(
        float pinchDelta)
    {
        Vector3 cameraPosition =
            studioCamera.transform.position;


        Vector3 productPosition =
            productRoot.position;


        Vector3 directionFromProduct =
            (
                cameraPosition -
                productPosition
            ).normalized;


        float currentDistance =
            Vector3.Distance(
                cameraPosition,
                productPosition
            );


        float zoomAmount =
            pinchDelta *
            zoomSpeed;


        float newDistance =
            currentDistance -
            zoomAmount;


        newDistance =
            Mathf.Clamp(
                newDistance,
                minCameraDistance,
                maxCameraDistance
            );


        studioCamera.transform.position =
            productPosition +
            directionFromProduct *
            newDistance;
    }


    // ========================================
    // PAN
    // ========================================

    private void ApplyPan(
        Vector2 delta)
    {
        Vector3 movement =
            (
                studioCamera.transform.right *
                delta.x
            +
                studioCamera.transform.up *
                delta.y
            ) *
            panSpeed;


        productRoot.position +=
            movement;
    }


    // ========================================
    // RESET
    // ========================================

    private void SaveInitialView()
    {
        if (studioCamera != null)
        {
            initialCameraPosition =
                studioCamera.transform.position;

            initialCameraRotation =
                studioCamera.transform.rotation;
        }


        if (productRoot != null)
        {
            initialProductPosition =
                productRoot.position;

            initialProductRotation =
                productRoot.rotation;
        }
    }


    public void ResetView()
    {
        isDragging = false;

        twoFingerGestureActive =
            false;


        if (studioCamera != null)
        {
            studioCamera.transform.position =
                initialCameraPosition;

            studioCamera.transform.rotation =
                initialCameraRotation;
        }


        if (productRoot != null)
        {
            productRoot.position =
                initialProductPosition;

            productRoot.rotation =
                initialProductRotation;
        }
    }
}