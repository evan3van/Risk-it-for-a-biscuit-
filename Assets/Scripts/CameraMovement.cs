using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam; // Make sure this is assigned in the Unity Inspector

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize;

    private Vector3 dragOrigin;
    private Vector3 origin;

    private float maxDragDistanceX = 100f;
    private float maxDragDistanceY = 100f;
    public float movementZoomStepX = 50f;
    public float movementZoomStepY = 30f;

    private void Start() 
    {
        origin = cam.transform.position;
    }

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


        //Work on this for max drag positions
        if(cam.transform.position.y > (origin.y+maxDragDistanceY))
        {
            cam.transform.position = new Vector3(cam.transform.position.x,origin.y + maxDragDistanceY,cam.transform.position.z);
        }
        else if(cam.transform.position.y < (origin.y-maxDragDistanceY))
        {
            cam.transform.position = new Vector3(cam.transform.position.x,origin.y - maxDragDistanceY,cam.transform.position.z);
        }
        if(cam.transform.position.x > (origin.x + maxDragDistanceX))
        {
            cam.transform.position = new Vector3(origin.x + maxDragDistanceX,cam.transform.position.y,cam.transform.position.z);
        }
        else if(cam.transform.position.x < (origin.x - maxDragDistanceX))
        {
            cam.transform.position = new Vector3(origin.x - maxDragDistanceX,cam.transform.position.y,cam.transform.position.z);
        }
        
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

            //Debug.Log("origin" + dragOrigin + " newPosition" + cam.ScreenToWorldPoint(Input.mousePosition) + " = difference" + difference);

            cam.transform.position += difference;
        }
    }

    
    public void ZoomIn() 
    {
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

        maxDragDistanceX += movementZoomStepX;
        maxDragDistanceY += movementZoomStepY;

        if(maxDragDistanceX < 100)
        {
            maxDragDistanceX = 100;
        }
        else if(maxDragDistanceX > 550)
        {
            maxDragDistanceX = 550;
        }

        if(maxDragDistanceY < 100)
        {
            maxDragDistanceY = 100;
        }
        else if(maxDragDistanceY > 550)
        {
            maxDragDistanceY = 550;
        }
        //Debug.Log(maxDragDistanceX);
    }


    public void ZoomOut() 
    {
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

        maxDragDistanceX -= movementZoomStepX;
        maxDragDistanceY -= movementZoomStepY;

        if(maxDragDistanceX < 100)
        {
            maxDragDistanceX = 100;
        }
        else if(maxDragDistanceX > 550)
        {
            maxDragDistanceX = 550;
        }

        if(maxDragDistanceY < 100)
        {
            maxDragDistanceY = 100;
        }
        else if(maxDragDistanceY > 550)
        {
            maxDragDistanceY = 550;
        }
        //Debug.Log(maxDragDistanceX);
    }

}

