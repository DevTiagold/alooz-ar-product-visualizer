using UnityEngine;
using UnityEngine.UIElements;


public class PrototypeUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private UIDocument uiDocument;


    [Header("AR")]
    [SerializeField]
    private ARPlacementController placementController;


    private Slider scaleSlider;

    private Button rotateLeftButton;
    private Button rotateRightButton;
    private Button lockButton;


    private Label rotationValue;

    private Label reticleStatus;

    private VisualElement reticleLayer;
    private VisualElement reticleRing;


    private float currentRotation = 0f;


    private void OnEnable()
    {
        if (uiDocument == null)
            return;


        VisualElement root =
            uiDocument.rootVisualElement;


        scaleSlider =
            root.Q<Slider>(
                "scale-slider"
            );


        rotateLeftButton =
            root.Q<Button>(
                "rotate-left-button"
            );


        rotateRightButton =
            root.Q<Button>(
                "rotate-right-button"
            );


        lockButton =
            root.Q<Button>(
                "lock-button"
            );


        rotationValue =
            root.Q<Label>(
                "rotation-value"
            );


        reticleLayer =
            root.Q<VisualElement>(
                "reticle-layer"
            );


        reticleRing =
            root.Q<VisualElement>(
                "reticle-ring"
            );


        reticleStatus =
            root.Q<Label>(
                "reticle-status"
            );


        /*
         * A retícula é apenas visual.
         * Ela não deve capturar toques.
         */
        if (reticleLayer != null)
        {
            reticleLayer.pickingMode =
                PickingMode.Ignore;
        }


        if (scaleSlider != null)
        {
            scaleSlider
                .RegisterValueChangedCallback(
                    OnScaleChanged
                );
        }


        if (rotateLeftButton != null)
        {
            rotateLeftButton.clicked +=
                RotateLeft;
        }


        if (rotateRightButton != null)
        {
            rotateRightButton.clicked +=
                RotateRight;
        }


        if (lockButton != null)
        {
            lockButton.clicked +=
                ToggleLock;
        }
    }


    private void OnDisable()
    {
        if (scaleSlider != null)
        {
            scaleSlider
                .UnregisterValueChangedCallback(
                    OnScaleChanged
                );
        }


        if (rotateLeftButton != null)
        {
            rotateLeftButton.clicked -=
                RotateLeft;
        }


        if (rotateRightButton != null)
        {
            rotateRightButton.clicked -=
                RotateRight;
        }


        if (lockButton != null)
        {
            lockButton.clicked -=
                ToggleLock;
        }
    }


    private void Update()
    {
        UpdateReticle();

        UpdateLockButton();
    }


    // ========================================
    // RETÍCULA
    // ========================================

    private void UpdateReticle()
    {
        if (
            placementController == null ||
            reticleLayer == null ||
            reticleRing == null
        )
        {
            return;
        }


        /*
         * Quando estiver fixado,
         * a retícula some.
         */
        if (placementController.IsLocked)
        {
            reticleLayer.style.display =
                DisplayStyle.None;

            return;
        }


        reticleLayer.style.display =
            DisplayStyle.Flex;


        reticleRing.RemoveFromClassList(
            "reticle-searching"
        );


        reticleRing.RemoveFromClassList(
            "reticle-valid"
        );


        if (
            placementController
                .HasValidPlacementPose
        )
        {
            reticleRing.AddToClassList(
                "reticle-valid"
            );


            if (reticleStatus != null)
            {
                reticleStatus.text =
                    placementController.HasObject
                    ? "Toque para mover"
                    : "Toque para posicionar";
            }
        }
        else
        {
            reticleRing.AddToClassList(
                "reticle-searching"
            );


            if (reticleStatus != null)
            {
                reticleStatus.text =
                    "Procurando superfície...";
            }
        }
    }


    // ========================================
    // SCALE
    // ========================================

    private void OnScaleChanged(
        ChangeEvent<float> evt)
    {
        if (placementController == null)
            return;


        GameObject target =
            placementController.CurrentObject;


        if (target == null)
            return;


        target.transform.localScale =
            Vector3.one *
            evt.newValue;
    }


    // ========================================
    // ROTATION
    // ========================================

    private void RotateLeft()
    {
        RotateObject(-15f);
    }


    private void RotateRight()
    {
        RotateObject(15f);
    }


    private void RotateObject(
        float angle)
    {
        if (placementController == null)
            return;


        GameObject target =
            placementController.CurrentObject;


        if (target == null)
            return;


        target.transform.Rotate(
            Vector3.up,
            angle,
            Space.World
        );


        currentRotation =
            Mathf.Repeat(
                currentRotation + angle,
                360f
            );


        if (rotationValue != null)
        {
            rotationValue.text =
                $"{Mathf.RoundToInt(currentRotation)}°";
        }
    }


    // ========================================
    // LOCK
    // ========================================

    private void ToggleLock()
    {
        if (placementController == null)
            return;


        if (!placementController.HasObject)
            return;


        if (placementController.IsLocked)
        {
            placementController
                .UnlockPlacement();


            if (lockButton != null)
            {
                lockButton.text =
                    "Fixar objeto";
            }
        }
        else
        {
            placementController
                .LockPlacement();


            if (lockButton != null)
            {
                lockButton.text =
                    "Editar posição";
            }
        }
    }


    private void UpdateLockButton()
    {
        if (
            placementController == null ||
            lockButton == null
        )
        {
            return;
        }


        lockButton.SetEnabled(
            placementController.HasObject
        );
    }
}