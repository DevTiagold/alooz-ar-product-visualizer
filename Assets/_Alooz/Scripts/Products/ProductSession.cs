using UnityEngine;

public static class ProductSession
{
    public static Color FaceColor { get; private set; }
        = Color.white;

    public static Color BodyColor { get; private set; }
        = Color.black;

    public static bool LightEnabled { get; private set; }
        = false;


    public static void Save(
        Color faceColor,
        Color bodyColor,
        bool lightEnabled)
    {
        FaceColor =
            faceColor;

        BodyColor =
            bodyColor;

        LightEnabled =
            lightEnabled;
    }


    public static void Reset()
    {
        FaceColor =
            Color.white;

        BodyColor =
            Color.black;

        LightEnabled =
            false;
    }
}