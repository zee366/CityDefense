using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PlayerTransform;

    public float minHeight = 15f;
    public float maxHeight = 100f;
    public float maxHeight_epsilon = 3f;
    public float orthoSize = 250f;
    public float scrollStep = 30f;
    public Quaternion squadViewOrientation = Quaternion.Euler(60, 0, 0);
    public Quaternion strategicViewOrientation = Quaternion.Euler(90, 0, 0);

    private Vector3 _cameraOffset;

    public bool LookAtPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
    }

    void LateUpdate()
    {
        float scrollwheel_delta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollwheel_delta != 0)     // If scrolling...
        {
            float radius = scrollwheel_delta * scrollStep;        //The radius from the camera

            float PosX = (Camera.main.transform.eulerAngles.x + 90) / 180 * Mathf.PI;
            float PosY = -(Camera.main.transform.eulerAngles.y - 90) / 180 * Mathf.PI;

            // Deltas along the radius from the camera
            float dX = radius * Mathf.Sin(PosX) * Mathf.Cos(PosY);
            float dY = radius * Mathf.Cos(PosX);
            float dZ = radius * Mathf.Sin(PosX) * Mathf.Sin(PosY);

            // Get current camera coordinate
            float CamX = Camera.main.transform.position.x;
            float CamY = Camera.main.transform.position.y;
            float CamZ = Camera.main.transform.position.z;

            // Update camera position only when it is above min height or if zooming out
            if (scrollwheel_delta < 0 || CamY + dY > minHeight)
                Camera.main.transform.position = new Vector3(CamX + dX, CamY + dY, CamZ + dZ);
        }

        // Strategic view
        if (Camera.main.transform.position.y >= maxHeight)
        {
            LookAtPlayer = false;
            Camera.main.orthographic = true;
            Camera.main.transform.position = new Vector3(0, maxHeight, 0);
            Camera.main.transform.localRotation = strategicViewOrientation;
            Camera.main.orthographicSize = orthoSize;
        }

        // Squad View
        if (Camera.main.transform.position.y < maxHeight)
        {
            LookAtPlayer = true;
            Camera.main.transform.localRotation = squadViewOrientation;
            Camera.main.orthographic = false;

            Vector3 newPos = new Vector3((PlayerTransform.position + _cameraOffset).x, Camera.main.transform.position.y, (PlayerTransform.position + _cameraOffset).z);

            transform.position = Vector3.Slerp(transform.position, newPos, 1.0f);

            if (LookAtPlayer)
                transform.LookAt(PlayerTransform);
        }

    }
}
