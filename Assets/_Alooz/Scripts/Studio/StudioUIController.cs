using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StudioUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private UIDocument uiDocument;


    [Header("Studio")]
    [SerializeField]
    private StudioProductController studioController;


    [Header("Product")]
    [SerializeField]
    private FrontlightProductController productController;


    [Header("Scenes")]
    [SerializeField]
    private string arSceneName = "Prototype";


    // ========================================
    // GENERAL UI
    // ========================================

    private Button resetButton;
    private Button arButton;

    private Button sheetToggle;
    private VisualElement sheetContent;


    // ========================================
    // FACE COLORS
    // ========================================

    private Button faceWhite;
    private Button faceRed;
    private Button faceBlue;
    private Button faceOrange;


    // ========================================
    // BODY COLORS
    // ========================================

    private Button bodyBlack;
    private Button bodyWhite;
    private Button bodyGray;
    private Button bodyBlue;


    // ========================================
    // LIGHT
    // ========================================

    private Button lightButton;


    // ========================================
    // STATE
    // ========================================

    private bool sheetExpanded = false;


    // ========================================
    // UNITY
    // ========================================

    private void OnEnable()
    {
        if (uiDocument == null)
        {
            Debug.LogError(
                "StudioUIController: UIDocument não foi atribuído."
            );

            return;
        }


        VisualElement root =
            uiDocument.rootVisualElement;


        if (root == null)
        {
            Debug.LogError(
                "StudioUIController: rootVisualElement não encontrado."
            );

            return;
        }


        CacheUI(root);

        RegisterCallbacks();

        RefreshUI();
    }


    private void OnDisable()
    {
        UnregisterCallbacks();
    }


    // ========================================
    // FIND UI ELEMENTS
    // ========================================

    private void CacheUI(
        VisualElement root)
    {
        // GENERAL

        resetButton =
            root.Q<Button>(
                "reset-button"
            );


        arButton =
            root.Q<Button>(
                "ar-button"
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


        // LIGHT

        lightButton =
            root.Q<Button>(
                "light-button"
            );
    }


    // ========================================
    // CALLBACK REGISTRATION
    // ========================================

    private void RegisterCallbacks()
    {
        if (resetButton != null)
            resetButton.clicked +=
                OnResetClicked;


        if (arButton != null)
            arButton.clicked +=
                OnARClicked;


        if (sheetToggle != null)
            sheetToggle.clicked +=
                ToggleSheet;


        // FACE COLORS

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


        // BODY COLORS

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
                OnLightClicked;
    }


    private void UnregisterCallbacks()
    {
        if (resetButton != null)
            resetButton.clicked -=
                OnResetClicked;


        if (arButton != null)
            arButton.clicked -=
                OnARClicked;


        if (sheetToggle != null)
            sheetToggle.clicked -=
                ToggleSheet;


        // FACE COLORS

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


        // BODY COLORS

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
                OnLightClicked;
    }


    // ========================================
    // BOTTOM SHEET
    // ========================================

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
            {
                sheetToggle.text =
                    "⌄";
            }
        }
        else
        {
            sheetContent.AddToClassList(
                "collapsed"
            );


            if (sheetToggle != null)
            {
                sheetToggle.text =
                    "⌃";
            }
        }
    }


    // ========================================
    // RESET
    // ========================================

    private void OnResetClicked()
    {
        if (studioController == null)
        {
            Debug.LogWarning(
                "StudioUIController: StudioProductController não atribuído."
            );

            return;
        }


        studioController.ResetView();
    }


    // ========================================
    // FACE COLORS
    // ========================================

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
                1.00f,
                0.71f,
                0.40f
            )
        );
    }


    private void SetFaceColor(
        Color color)
    {
        if (productController == null)
        {
            Debug.LogWarning(
                "StudioUIController: FrontlightProductController não atribuído."
            );

            return;
        }


        productController.SetFaceColor(
            color
        );
    }


    // ========================================
    // BODY COLORS
    // ========================================

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
        if (productController == null)
        {
            Debug.LogWarning(
                "StudioUIController: FrontlightProductController não atribuído."
            );

            return;
        }


        productController.SetBodyColor(
            color
        );
    }


    // ========================================
    // LIGHT
    // ========================================

    private void OnLightClicked()
    {
        if (productController == null)
        {
            Debug.LogWarning(
                "StudioUIController: FrontlightProductController não atribuído."
            );

            return;
        }


        productController.ToggleLight();

        RefreshLightButton();
    }


    private void RefreshLightButton()
    {
        if (
            lightButton == null ||
            productController == null
        )
        {
            return;
        }


        bool enabled =
            productController.LightEnabled;


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


    // ========================================
    // AR
    // ========================================

    private void OnARClicked()
    {
        if (productController == null)
        {
            Debug.LogError(
                "StudioUIController: ProductController não configurado."
            );

            return;
        }


        ProductSession.Save(
            productController.FaceColor,
            productController.BodyColor,
            productController.LightEnabled
        );


        if (string.IsNullOrEmpty(arSceneName))
        {
            Debug.LogError(
                "StudioUIController: nome da cena AR não configurado."
            );

            return;
        }


        SceneManager.LoadScene(
            arSceneName
        );
    }

    // ========================================
    // REFRESH
    // ========================================

    private void RefreshUI()
    {
        RefreshSheet();

        RefreshLightButton();
    }
}