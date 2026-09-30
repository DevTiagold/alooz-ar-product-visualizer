using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARReticleController : MonoBehaviour
{
    [Header("AR")]
    [SerializeField]
    private ARRaycastManager raycastManager;


    [Header("Reticle")]
    [SerializeField]
    private GameObject reticleObject;


    private static readonly List<ARRaycastHit> hits =
        new List<ARRaycastHit>();


    private Pose currentPose;

    private bool reticleEnabled = true;


    public bool HasValidPose { get; private set; }

    public Pose CurrentPose =>
        currentPose;


    private void Update()
    {
        UpdateReticle();
    }


    // ============================================================
    // UPDATE RETICLE
    // ============================================================

    private void UpdateReticle()
    {
        if (!reticleEnabled)
        {
            HasValidPose = false;


            if (reticleObject != null)
            {
                reticleObject.SetActive(
                    false
                );
            }


            return;
        }


        if (raycastManager == null)
        {
            HasValidPose = false;


            if (reticleObject != null)
            {
                reticleObject.SetActive(
                    false
                );
            }


            return;
        }


        Vector2 screenCenter =
            new Vector2(
                Screen.width * 0.5f,
                Screen.height * 0.5f
            );


        bool hitFound =
            raycastManager.Raycast(
                screenCenter,
                hits,
                TrackableType.PlaneWithinPolygon
            );


        HasValidPose =
            hitFound;


        if (!hitFound)
        {
            if (reticleObject != null)
            {
                reticleObject.SetActive(
                    false
                );
            }


            return;
        }


        currentPose =
            hits[0].pose;


        if (reticleObject == null)
            return;


        reticleObject.SetActive(
            true
        );


        reticleObject.transform
            .SetPositionAndRotation(
                currentPose.position,
                currentPose.rotation
            );
    }


    // ============================================================
    // ENABLE / DISABLE
    // ============================================================

    public void SetReticleEnabled(
        bool enabled)
    {
        reticleEnabled =
            enabled;


        if (!enabled)
        {
            HasValidPose = false;


            if (reticleObject != null)
            {
                reticleObject.SetActive(
                    false
                );
            }
        }
    }
}