using UnityEngine;

public class OcclusionCullingSetup : MonoBehaviour
{
    public Camera topDownCamera;
    public Camera firstPersonCamera;

    void Start()
    {
        if (topDownCamera == null)
            topDownCamera = Camera.main; // Assign default if not set

        if (firstPersonCamera == null)
            Debug.LogWarning("First-person camera not assigned!");

        EnableCulling(topDownCamera);
        EnableCulling(firstPersonCamera);
    }

    void EnableCulling(Camera cam)
    {
        if (cam != null)
        {
            cam.useOcclusionCulling = true;
            Debug.Log($"Enabled Occlusion Culling for {cam.name}");
        }
    }
}