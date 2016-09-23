using UnityEngine;
using System.Collections;
using Improbable.Checks;
using Improbable.Unity.Visualizer;

public class PlayerCameraVisualizer : MonoBehaviour
{

    [Require]
    CheckIsClientSideWriter checkIsClientSideWriter;

    public Camera myCamera;

    void OnEnable()
    {
        if (this.myCamera == null)
        {
            myCamera = this.GetComponentInChildren<Camera>();
        }
        
        if (myCamera != null)
        {
            myCamera.enabled = true;
        }
        
    }

    void OnDisable()
    {
        if (myCamera != null)
        {
            myCamera.enabled = false;
        }
    }
}
