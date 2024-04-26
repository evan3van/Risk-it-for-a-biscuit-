using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Camera))]
public class CameraOutline : Editor
{
    private void OnSceneGUI()
    {
        Camera camera = target as Camera;

        // Draw camera outline
        Handles.DrawWireDisc(camera.transform.position, Vector3.back, camera.orthographicSize * camera.aspect);
    }
}