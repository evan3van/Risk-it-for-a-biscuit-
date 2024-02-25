using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam; // Make sure this is assigned in the Unity Inspector

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize;

    private Vector3 dragOrigin;

    private int maxDragDistanceY;
    private int maxDragDistanceX;

    // Unity uses "Update" with an uppercase "U" for its built-in method.
    private void Update() 
    {
        PanCamera();

        if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // forward
        {
            ZoomIn();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backwards
        {
            ZoomOut();
        }

        /*    Work on this for max drag positions
        if(cam.transform.position.y > (dragOrigin.y+maxDragDistanceY))
        {
            cam.transform.position = new Vector3(cam.transform.position.x,dragOrigin.y+maxDragDistanceY,cam.transform.position.z);
        }
        else if(cam.transform.position.y < (dragOrigin.y-maxDragDistanceY))
        {
            cam.transform.position = new Vector3(cam.transform.position.x,dragOrigin.y-maxDragDistanceY,cam.transform.position.z);
        }
        */
    }

    private void PanCamera()
    {
        // Use "Input" with an uppercase "I", "GetMouseButtonDown" with proper casing
        if (Input.GetMouseButtonDown(0))
        {
            // "cam.ScreenToWorldPoint" with proper casing and method name
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            // Correct to only consider the X and Y, setting Z to a fixed value might be needed depending on your camera setup
            dragOrigin.z = cam.transform.position.z;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            difference.z = 0; // Assuming you only want to pan in the X and Y directions

            Debug.Log("origin" + dragOrigin + " newPosition" + cam.ScreenToWorldPoint(Input.mousePosition) + " = difference" + difference);

            cam.transform.position += difference;
        }
    }

    
    public void ZoomIn() 
    {
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }


    public void ZoomOut() 
    {
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }

}

