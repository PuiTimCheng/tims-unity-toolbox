using UnityEngine;
[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
    public Camera targetCamera;

    void Start()
    {
        // If no camera is set, default to the main camera
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        // Make the object face the camera
        transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward,
            targetCamera.transform.rotation * Vector3.up);
    }
}