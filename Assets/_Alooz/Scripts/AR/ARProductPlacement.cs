using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

using Touch =
    UnityEngine.InputSystem.EnhancedTouch.Touch;


public class ARProductPlacement : MonoBehaviour
{
    [Header("Reticle")]
    [SerializeField]
    private ARReticleController reticleController;


    [Header("AR")]
    [SerializeField]
    private ARAnchorManager anchorManager;


    [Header("Product")]
    [SerializeField]
    private GameObject productPrefab;


    [Header("UI")]
    [SerializeField]
    private UIDocument uiDocument;


    private GameObject spawnedProduct;

    private ARAnchor currentAnchor;

    private bool placementLocked;


    // ============================================================
    // PUBLIC
    // ============================================================

    public GameObject CurrentProduct =>
        spawnedProduct;


    public bool HasProduct =>
        spawnedProduct != null;


    public bool IsLocked =>
        placementLocked;


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
        HandleTouch();
    }


    // ============================================================
    // TOUCH
    // ============================================================

    private void HandleTouch()
    {
        if (placementLocked)
            return;


        if (reticleController == null)
            return;


        if (!reticleController.HasValidPose)
            return;


        if (Touch.activeTouches.Count == 0)
            return;


        Touch touch =
            Touch.activeTouches[0];


        if (
            touch.phase !=
            UnityEngine.InputSystem.TouchPhase.Began
        )
        {
            return;
        }


        if (
            IsPointerOverUI(
                touch.screenPosition
            )
        )
        {
            return;
        }


        PlaceOrMoveProduct();
    }


    // ============================================================
    // PLACE / MOVE
    // ============================================================

    private void PlaceOrMoveProduct()
    {
        Pose placementPose =
            reticleController.CurrentPose;


        CreateOrReplaceAnchor(
            placementPose
        );


        if (currentAnchor == null)
        {
            Debug.LogWarning(
                "ARProductPlacement: não foi possível criar o ARAnchor."
            );

            return;
        }


        /*
         * Primeiro posicionamento
         */
        if (spawnedProduct == null)
        {
            if (productPrefab == null)
            {
                Debug.LogError(
                    "ARProductPlacement: Product Prefab não foi atribuído."
                );

                return;
            }


            spawnedProduct =
                Instantiate(
                    productPrefab,
                    currentAnchor.transform
                );


            /*
             * O produto fica na origem local do Anchor.
             * Como o pivot do M está na base,
             * ele deve encostar na superfície.
             */
            spawnedProduct.transform.localPosition =
                Vector3.zero;


            spawnedProduct.transform.localRotation =
                Quaternion.identity;


            ApplyStudioConfiguration();


            return;
        }


        /*
         * Reposicionamento:
         * o produto passa a ser filho do novo Anchor.
         */
        spawnedProduct.transform.SetParent(
            currentAnchor.transform,
            false
        );


        spawnedProduct.transform.localPosition =
            Vector3.zero;


        spawnedProduct.transform.localRotation =
            Quaternion.identity;
    }


    // ============================================================
    // ANCHOR
    // ============================================================

    private void CreateOrReplaceAnchor(
        Pose pose)
    {
        if (anchorManager == null)
        {
            Debug.LogError(
                "ARProductPlacement: ARAnchorManager não foi atribuído."
            );

            return;
        }


        /*
         * Remove o Anchor anterior ao reposicionar.
         */
        if (currentAnchor != null)
        {
            Destroy(
                currentAnchor.gameObject
            );

            currentAnchor =
                null;
        }


        GameObject anchorObject =
            new GameObject(
                "ProductAnchor"
            );


        anchorObject.transform.SetPositionAndRotation(
            pose.position,
            pose.rotation
        );


        currentAnchor =
            anchorObject.AddComponent<ARAnchor>();
    }


    // ============================================================
    // STUDIO CONFIGURATION
    // ============================================================

    private void ApplyStudioConfiguration()
    {
        if (spawnedProduct == null)
            return;


        FrontlightProductController controller =
            spawnedProduct
                .GetComponentInChildren
                <FrontlightProductController>();


        if (controller == null)
        {
            Debug.LogWarning(
                "ARProductPlacement: " +
                "FrontlightProductController não encontrado."
            );

            return;
        }


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
        if (spawnedProduct == null)
            return;


        placementLocked = true;


        if (reticleController != null)
        {
            reticleController.SetReticleEnabled(
                false
            );
        }
    }


    public void UnlockPlacement()
    {
        if (spawnedProduct == null)
            return;


        placementLocked = false;


        if (reticleController != null)
        {
            reticleController.SetReticleEnabled(
                true
            );
        }
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


        Vector2 panelPosition =
            RuntimePanelUtils.ScreenToPanel(
                root.panel,
                screenPosition
            );


        VisualElement pickedElement =
            root.panel.Pick(
                panelPosition
            );


        while (pickedElement != null)
        {
            if (
                pickedElement.ClassListContains(
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