using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 origin;
    private bool cameraDragging = false;

    private void Awake() 
    {
        mainCamera = Camera.main;
        origin = mainCamera.transform.position;
    }
}
