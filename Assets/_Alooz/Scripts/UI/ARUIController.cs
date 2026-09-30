using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class ARUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private UIDocument uiDocument;


    [Header("AR")]
    [SerializeField]
    private ARProductPlacement productPlacement;


    [Header("Scenes")]
    [SerializeField]
    private string studioSceneName =
        "StudioScene";


    // ============================================================
    // GENERAL
    // ============================================================

    private Button backButton;

    private Button sheetToggle;

    private VisualElement sheetContent;


    // ============================================================
    // COLORS
    // ============================================================

    private Button faceWhite;
    private Button faceRed;
    private Button faceBlue;
    private Button faceOrange;


    private Button bodyBlack;
    private Button bodyWhite;
    private Button bodyGray;
    private Button bodyBlue;


    // ============================================================
    // PRODUCT
    // ============================================================

    private Button lightButton;

    private Button lockButton;

    private Button studioButton;


    // ============================================================
    // STATE
    // ============================================================

    private bool sheetExpanded;


    // ============================================================
    // UNITY
    // ============================================================

    private void OnEnable()
    {
        if (uiDocument == null)
        {
            Debug.LogError(
                "ARUIController: UIDocument não atribuído."
            );

            return;
        }


        VisualElement root =
            uiDocument.rootVisualElement;


        CacheUI(
            root
        );


        RegisterCallbacks();


        RefreshUI();
    }


    private void OnDisable()
    {
        UnregisterCallbacks();
    }


    // ============================================================
    // CACHE
    // ============================================================

    private void CacheUI(
        VisualElement root)
    {
        backButton =
            root.Q<Button>(
                "back-button"
            );


        sheetToggle =
            root.Q<Button>(
                "sheet-toggle"
            );


        sheetContent =
            root.Q<VisualElement>(
                "sheet-content"
            );


        // FACE

        faceWhite =
            root.Q<Button>(
                "face-white"
            );


        faceRed =
            root.Q<Button>(
                "face-red"
            );


        faceBlue =
            root.Q<Button>(
                "face-blue"
            );


        faceOrange =
            root.Q<Button>(
                "face-orange"
            );


        // BODY

        bodyBlack =
            root.Q<Button>(
                "body-black"
            );


        bodyWhite =
            root.Q<Button>(
                "body-white"
            );


        bodyGray =
            root.Q<Button>(
                "body-gray"
            );


        bodyBlue =
            root.Q<Button>(
                "body-blue"
            );


        // PRODUCT

        lightButton =
            root.Q<Button>(
                "light-button"
            );


        lockButton =
            root.Q<Button>(
                "lock-button"
            );


        studioButton =
            root.Q<Button>(
                "studio-button"
            );
    }


    // ============================================================
    // CALLBACKS
    // ============================================================

    private void RegisterCallbacks()
    {
        if (backButton != null)
            backButton.clicked +=
                ReturnToStudio;


        if (studioButton != null)
            studioButton.clicked +=
                ReturnToStudio;


        if (sheetToggle != null)
            sheetToggle.clicked +=
                ToggleSheet;


        // FACE

        if (faceWhite != null)
            faceWhite.clicked +=
                SetFaceWhite;


        if (faceRed != null)
            faceRed.clicked +=
                SetFaceRed;


        if (faceBlue != null)
            faceBlue.clicked +=
                SetFaceBlue;


        if (faceOrange != null)
            faceOrange.clicked +=
                SetFaceOrange;


        // BODY

        if (bodyBlack != null)
            bodyBlack.clicked +=
                SetBodyBlack;


        if (bodyWhite != null)
            bodyWhite.clicked +=
                SetBodyWhite;


        if (bodyGray != null)
            bodyGray.clicked +=
                SetBodyGray;


        if (bodyBlue != null)
            bodyBlue.clicked +=
                SetBodyBlue;


        // LIGHT

        if (lightButton != null)
            lightButton.clicked +=
                ToggleLight;


        // LOCK

        if (lockButton != null)
            lockButton.clicked +=
                ToggleLock;
    }


    private void UnregisterCallbacks()
    {
        if (backButton != null)
            backButton.clicked -=
                ReturnToStudio;


        if (studioButton != null)
            studioButton.clicked -=
                ReturnToStudio;


        if (sheetToggle != null)
            sheetToggle.clicked -=
                ToggleSheet;


        // FACE

        if (faceWhite != null)
            faceWhite.clicked -=
                SetFaceWhite;


        if (faceRed != null)
            faceRed.clicked -=
                SetFaceRed;


        if (faceBlue != null)
            faceBlue.clicked -=
                SetFaceBlue;


        if (faceOrange != null)
            faceOrange.clicked -=
                SetFaceOrange;


        // BODY

        if (bodyBlack != null)
            bodyBlack.clicked -=
                SetBodyBlack;


        if (bodyWhite != null)
            bodyWhite.clicked -=
                SetBodyWhite;


        if (bodyGray != null)
            bodyGray.clicked -=
                SetBodyGray;


        if (bodyBlue != null)
            bodyBlue.clicked -=
                SetBodyBlue;


        // LIGHT

        if (lightButton != null)
            lightButton.clicked -=
                ToggleLight;


        // LOCK

        if (lockButton != null)
            lockButton.clicked -=
                ToggleLock;
    }


    // ============================================================
    // PRODUCT CONTROLLER
    // ============================================================

    private FrontlightProductController
        GetProductController()
    {
        if (productPlacement == null)
            return null;


        GameObject product =
            productPlacement.CurrentProduct;


        if (product == null)
            return null;


        return product.GetComponentInChildren
            <FrontlightProductController>();
    }


    // ============================================================
    // SHEET
    // ============================================================

    private void ToggleSheet()
    {
        sheetExpanded =
            !sheetExpanded;


        RefreshSheet();
    }


    private void RefreshSheet()
    {
        if (sheetContent == null)
            return;


        if (sheetExpanded)
        {
            sheetContent.RemoveFromClassList(
                "collapsed"
            );


            if (sheetToggle != null)
                sheetToggle.text = "⌄";
        }
        else
        {
            sheetContent.AddToClassList(
                "collapsed"
            );


            if (sheetToggle != null)
                sheetToggle.text = "⌃";
        }
    }


    // ============================================================
    // FACE
    // ============================================================

    private void SetFaceWhite()
    {
        SetFaceColor(
            Color.white
        );
    }


    private void SetFaceRed()
    {
        SetFaceColor(
            new Color(
                0.78f,
                0.18f,
                0.18f
            )
        );
    }


    private void SetFaceBlue()
    {
        SetFaceColor(
            new Color(
                0.14f,
                0.33f,
                0.63f
            )
        );
    }


    private void SetFaceOrange()
    {
        SetFaceColor(
            new Color(
                1f,
                0.71f,
                0.40f
            )
        );
    }


    private void SetFaceColor(
        Color color)
    {
        FrontlightProductController controller =
            GetProductController();


        if (controller == null)
            return;


        controller.SetFaceColor(
            color
        );


        SaveCurrentState(
            controller
        );
    }


    // ============================================================
    // BODY
    // ============================================================

    private void SetBodyBlack()
    {
        SetBodyColor(
            new Color(
                0.08f,
                0.08f,
                0.08f
            )
        );
    }


    private void SetBodyWhite()
    {
        SetBodyColor(
            Color.white
        );
    }


    private void SetBodyGray()
    {
        SetBodyColor(
            new Color(
                0.47f,
                0.47f,
                0.47f
            )
        );
    }


    private void SetBodyBlue()
    {
        SetBodyColor(
            new Color(
                0.14f,
                0.33f,
                0.63f
            )
        );
    }


    private void SetBodyColor(
        Color color)
    {
        FrontlightProductController controller =
            GetProductController();


        if (controller == null)
            return;


        controller.SetBodyColor(
            color
        );


        SaveCurrentState(
            controller
        );
    }


    // ============================================================
    // LIGHT
    // ============================================================

    private void ToggleLight()
    {
        FrontlightProductController controller =
            GetProductController();


        if (controller == null)
            return;


        controller.ToggleLight();


        SaveCurrentState(
            controller
        );


        RefreshLightButton();
    }


    private void RefreshLightButton()
    {
        if (lightButton == null)
            return;


        FrontlightProductController controller =
            GetProductController();


        /*
         * Antes do produto ser colocado,
         * mantemos o estado vindo do Studio.
         */
        bool enabled =
            controller != null
            ? controller.LightEnabled
            : ProductSession.LightEnabled;


        if (enabled)
        {
            lightButton.text =
                "DESLIGAR";


            lightButton.AddToClassList(
                "light-button-on"
            );
        }
        else
        {
            lightButton.text =
                "LIGAR";


            lightButton.RemoveFromClassList(
                "light-button-on"
            );
        }
    }


    // ============================================================
    // LOCK
    // ============================================================

    private void ToggleLock()
    {
        if (productPlacement == null)
            return;


        if (!productPlacement.HasProduct)
            return;


        if (productPlacement.IsLocked)
        {
            productPlacement.UnlockPlacement();
        }
        else
        {
            productPlacement.LockPlacement();
        }


        RefreshLockButton();
    }


    private void RefreshLockButton()
    {
        if (
            lockButton == null ||
            productPlacement == null
        )
        {
            return;
        }


        lockButton.SetEnabled(
            productPlacement.HasProduct
        );


        lockButton.text =
            productPlacement.IsLocked
            ? "Editar posição"
            : "Fixar objeto";
    }


    // ============================================================
    // SESSION
    // ============================================================

    private void SaveCurrentState(
        FrontlightProductController controller)
    {
        if (controller == null)
            return;


        ProductSession.Save(
            controller.FaceColor,
            controller.BodyColor,
            controller.LightEnabled
        );
    }


    // ============================================================
    // STUDIO
    // ============================================================

    private void ReturnToStudio()
    {
        /*
         * Salva o estado atual do produto
         * antes de voltar.
         */

        FrontlightProductController controller =
            GetProductController();


        if (controller != null)
        {
            SaveCurrentState(
                controller
            );
        }


        if (
            string.IsNullOrEmpty(
                studioSceneName
            )
        )
        {
            Debug.LogError(
                "ARUIController: Studio Scene não configurada."
            );

            return;
        }


        SceneManager.LoadScene(
            studioSceneName
        );
    }


    // ============================================================
    // REFRESH
    // ============================================================

    private void RefreshUI()
    {
        RefreshSheet();

        RefreshLightButton();

        RefreshLockButton();
    }


    private void Update()
    {
        /*
         * O produto pode ser criado depois
         * do carregamento da UI.
         *
         * Atualizamos apenas o botão que
         * depende da existência do produto.
         */
        RefreshLockButton();
    }
}