using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Controls the camera movement including panning and zooming within specified boundaries.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    /// <summary>
    /// The camera
    /// </summary>
    private Camera cam;

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize;

    /// <summary>
    /// Determines if the camera can move.
    /// </summary>
    public bool canMove = true;

    private Vector3 dragOrigin;
    private Vector3 origin;

    /// <summary>
    /// Indicates if dragging is currently active.
    /// </summary>
    
    public bool dragActive = true;
    private float maxDragDistanceX = 100f;
    private float maxDragDistanceY = 100f;

    /// <summary>
    /// Adjusts the drag distance based on zoom level for the X axis.
    /// </summary>
    
    public float movementZoomStepX = 50f;

    /// <summary>
    /// Adjusts the drag distance based on zoom level for the Y axis.
    /// </summary>
    
    public float movementZoomStepY = 30f;

    /// <summary>
    /// On initialisation of the script, the origin is set as the camera's current position.
    /// </summary>
    private void Start() 
    {
        origin = cam.transform.position;
    }

    /// <summary>
    /// Checks for player inputs once per frame, to allow zoom/pan functionality and checks if the camera 
    /// is out of the set boundary, if so, it resets it to within the boundary.
    /// </summary>
    private void Update() 
    {
        if(canMove)
        {
            PanCamera();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f )
        {
            ZoomIn();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f )
        {
            ZoomOut();
        }
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

    /// <summary>
    /// Checks for if the player has the left mouse button pressed and if so, calculates the
    /// distance the player has dragged the camera, and moves the camera in that direction.
    /// </summary>
    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            dragOrigin.z = cam.transform.position.z;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            difference.z = 0;
            cam.transform.position += difference;
        }
    }

    /// <summary>
    /// Causes the size of the camera to decrease based on the set <strong>zoomStep</strong> field in the inspector up to a minimum
    /// camera size <strong>minCamSize</strong>.
    /// Also increments the drag distance based on the set <strong>movementZoomStep</strong> fields for x and y in the inspector up to a maximum
    /// drag distance.
    /// </summary>
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
        
    }

    /// <summary>
    /// Causes the size of the camera to decrease based on the set <strong>zoomStep</strong> field in the inspector up to a maximum
    /// camera size <strong>maxCamSize</strong>.
    /// Also decrements the drag distance based on the set <strong>movementZoomStep</strong> fields for x and y in the inspector up to a maximum
    /// drag distance.
    /// </summary>
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
        
    }

}

