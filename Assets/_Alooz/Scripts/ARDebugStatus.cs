using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARDebugStatus : MonoBehaviour
{
    private ARSessionState lastState;

    private void Start()
    {
        lastState = ARSession.state;

        Debug.Log(
            $"AR DEBUG - Estado inicial: {lastState}"
        );
    }

    private void Update()
    {
        if (ARSession.state == lastState)
            return;

        lastState = ARSession.state;

        Debug.Log(
            $"AR DEBUG - Novo estado: {lastState}"
        );
    }
}