using UnityEngine;

public class MapScaler : MonoBehaviour
{
    public Camera mainCamera;
    private void Update() 
    {
        FitToCamera();
    }

    void FitToCamera()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Calculate the scale needed to fit the rectangle inside the camera
        Vector3 scale = transform.localScale;
        float scaleX = cameraWidth / scale.x;
        float scaleY = cameraHeight / scale.y;
        float minScale = Mathf.Min(scaleX, scaleY);

        // Apply the scaling to the rectangle object
        transform.localScale *= minScale;
    }
}
