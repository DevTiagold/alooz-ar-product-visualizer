using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using Touch =
    UnityEngine.InputSystem.EnhancedTouch.Touch;


public class ARPlacementController : MonoBehaviour
{
    // ============================================================
    // REFERENCES
    // ============================================================

    [Header("AR")]
    [SerializeField]
    private ARRaycastManager raycastManager;


    [Header("UI")]
    [SerializeField]
    private UIDocument uiDocument;


    [Header("Product")]
    [SerializeField]
    private GameObject placementPrefab;


    // ============================================================
    // RAYCAST
    // ============================================================

    private static readonly List<ARRaycastHit> hits =
        new List<ARRaycastHit>();


    private Pose currentPlacementPose;

    private bool hasValidPlacementPose;


    // ============================================================
    // PRODUCT
    // ============================================================

    private GameObject spawnedObject;

    private bool placementLocked;


    // ============================================================
    // PUBLIC PROPERTIES
    // ============================================================

    public GameObject CurrentObject =>
        spawnedObject;


    public bool HasObject =>
        spawnedObject != null;


    public bool IsLocked =>
        placementLocked;


    public bool HasValidPlacementPose =>
        hasValidPlacementPose;


    // ============================================================
    // UNITY
    // ============================================================

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
        UpdatePlacementPose();

        HandlePlacementTouch();
    }


    // ============================================================
    // PLACEMENT POSE
    // ============================================================

    private void UpdatePlacementPose()
    {
        /*
         * Se o produto estiver fixado,
         * não precisamos continuar oferecendo
         * um ponto válido para reposicionamento.
         */

        if (placementLocked)
        {
            hasValidPlacementPose = false;

            return;
        }


        if (raycastManager == null)
        {
            hasValidPlacementPose = false;

            return;
        }


        /*
         * O raycast acontece sempre no centro da tela.
         *
         * A retícula da UI também fica no centro.
         */

        Vector2 screenCenter =
            new Vector2(
                Screen.width * 0.5f,
                Screen.height * 0.5f
            );


        hasValidPlacementPose =
            raycastManager.Raycast(
                screenCenter,
                hits,
                TrackableType.PlaneWithinPolygon
            );


        if (!hasValidPlacementPose)
            return;


        currentPlacementPose =
            hits[0].pose;
    }


    // ============================================================
    // TOUCH
    // ============================================================

    private void HandlePlacementTouch()
    {
        if (placementLocked)
            return;


        if (!hasValidPlacementPose)
            return;


        if (Touch.activeTouches.Count == 0)
            return;


        /*
         * Para placement utilizamos apenas
         * o primeiro toque.
         */

        Touch touch =
            Touch.activeTouches[0];


        if (
            touch.phase !=
            UnityEngine.InputSystem.TouchPhase.Began
        )
        {
            return;
        }


        /*
         * Se o toque começou sobre uma região
         * da UI marcada como blocks-ar,
         * não fazemos placement.
         */

        if (
            IsPointerOverUI(
                touch.screenPosition
            )
        )
        {
            return;
        }


        PlaceOrMoveObject();
    }


    // ============================================================
    // PLACE / MOVE
    // ============================================================

    private void PlaceOrMoveObject()
    {
        if (placementPrefab == null)
        {
            Debug.LogError(
                "ARPlacementController: Placement Prefab não foi atribuído."
            );

            return;
        }


        /*
         * PRIMEIRO POSICIONAMENTO
         */

        if (spawnedObject == null)
        {
            spawnedObject =
                Instantiate(
                    placementPrefab,
                    currentPlacementPose.position,
                    currentPlacementPose.rotation
                );


            /*
             * Aplica a configuração feita
             * anteriormente no Studio.
             */

            ApplyProductConfiguration(
                spawnedObject
            );


            return;
        }


        /*
         * REPOSICIONAMENTO
         *
         * Enquanto não estiver fixado,
         * um novo toque move o mesmo produto
         * para a posição da retícula.
         */

        spawnedObject.transform
            .SetPositionAndRotation(
                currentPlacementPose.position,
                currentPlacementPose.rotation
            );
    }


    // ============================================================
    // PRODUCT CONFIGURATION
    // ============================================================

    private void ApplyProductConfiguration(
        GameObject product)
    {
        if (product == null)
            return;


        FrontlightProductController controller =
            product.GetComponentInChildren
            <FrontlightProductController>();


        if (controller == null)
        {
            Debug.LogWarning(
                "ARPlacementController: " +
                "FrontlightProductController não encontrado no produto."
            );

            return;
        }


        /*
         * Aplica exatamente o estado escolhido
         * no Studio antes da troca de cena.
         */

        controller.SetFaceColor(
            ProductSession.FaceColor
        );


        controller.SetBodyColor(
            ProductSession.BodyColor
        );


        controller.SetLightEnabled(
            ProductSession.LightEnabled
        );
    }


    // ============================================================
    // LOCK
    // ============================================================

    public void LockPlacement()
    {
        if (spawnedObject == null)
            return;


        placementLocked = true;

        hasValidPlacementPose = false;
    }


    public void UnlockPlacement()
    {
        if (spawnedObject == null)
            return;


        placementLocked = false;
    }


    // ============================================================
    // UI HIT TEST
    // ============================================================

    private bool IsPointerOverUI(
        Vector2 screenPosition)
    {
        if (uiDocument == null)
            return false;


        VisualElement root =
            uiDocument.rootVisualElement;


        if (
            root == null ||
            root.panel == null
        )
        {
            return false;
        }


        /*
         * Converte a posição do toque
         * da tela para coordenadas do UI Toolkit.
         */

        Vector2 panelPosition =
            RuntimePanelUtils.ScreenToPanel(
                root.panel,
                screenPosition
            );


        VisualElement pickedElement =
            root.panel.Pick(
                panelPosition
            );


        /*
         * Percorremos o elemento tocado
         * e seus pais procurando a classe
         * blocks-ar.
         */

        while (pickedElement != null)
        {
            if (
                pickedElement
                    .ClassListContains(
                        "blocks-ar"
                    )
            )
            {
                return true;
            }


            pickedElement =
                pickedElement.parent;
        }


        return false;
    }
}