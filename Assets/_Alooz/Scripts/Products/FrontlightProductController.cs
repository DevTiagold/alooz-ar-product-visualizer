using UnityEngine;

public class FrontlightProductController : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField]
    private Renderer faceRenderer;

    [SerializeField]
    private Renderer bodyRenderer;


    [Header("Initial State")]
    [SerializeField]
    private Color initialFaceColor = Color.white;

    [SerializeField]
    private Color initialBodyColor = Color.black;

    [SerializeField]
    private bool lightEnabled = false;


    [Header("Frontlight")]
    [SerializeField]
    [Min(0f)]
    private float emissionIntensity = 5f;


    private Material faceMaterial;
    private Material bodyMaterial;

    private Color faceColor;
    private Color bodyColor;


    public Color FaceColor => faceColor;
    public Color BodyColor => bodyColor;
    public bool LightEnabled => lightEnabled;


    private void Awake()
    {
        if (faceRenderer != null)
        {
            faceMaterial =
                faceRenderer.material;
        }

        if (bodyRenderer != null)
        {
            bodyMaterial =
                bodyRenderer.material;
        }


        faceColor =
            initialFaceColor;

        bodyColor =
            initialBodyColor;


        ApplyAll();
    }


    // ========================================
    // FACE COLOR
    // ========================================

    public void SetFaceColor(Color color)
    {
        faceColor = color;

        ApplyFace();

        // Se estiver acesa, a emissão acompanha
        // imediatamente a nova cor.
        ApplyLighting();
    }


    // ========================================
    // BODY COLOR
    // ========================================

    public void SetBodyColor(Color color)
    {
        bodyColor = color;

        ApplyBody();
    }


    // ========================================
    // LIGHT
    // ========================================

    public void SetLightEnabled(bool enabled)
    {
        lightEnabled = enabled;

        ApplyLighting();
    }


    public void ToggleLight()
    {
        SetLightEnabled(
            !lightEnabled
        );
    }


    // ========================================
    // APPLY
    // ========================================

    private void ApplyAll()
    {
        ApplyFace();
        ApplyBody();
        ApplyLighting();
    }


    private void ApplyFace()
    {
        if (faceMaterial == null)
            return;


        faceMaterial.SetColor(
            "_BaseColor",
            faceColor
        );
    }


    private void ApplyBody()
    {
        if (bodyMaterial == null)
            return;


        bodyMaterial.SetColor(
            "_BaseColor",
            bodyColor
        );
    }


    private void ApplyLighting()
    {
        if (faceMaterial == null)
            return;


        Color emissionColor;

        if (lightEnabled)
        {
            emissionColor =
                faceColor *
                emissionIntensity;
        }
        else
        {
            emissionColor =
                Color.black;
        }


        faceMaterial.SetColor(
            "_EmissionColor",
            emissionColor
        );
    }
}