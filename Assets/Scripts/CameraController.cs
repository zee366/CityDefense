using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 OrthoOffset = new Vector3(0f,0f,0f);
    public float minHeight = 15f;
    public float maxHeight = 100f;
    public float maxHeight_epsilon = 3f;
    public float orthoSize = 250f;
    public float scrollStep = 30f;

    private Vector3 _cameraOffset;

    public bool LookAtPlayer = false;

    GameObject target; 

    // Start is called before the first frame update
    void Start() {
        GameObject policeFlock = GameObject.Find("PoliceFlock");
        if(policeFlock == null)
            Debug.LogError("Could not find GameObject by name 'PoliceFlock'. Required for this CameraController.");

        target = GetChildWithName(policeFlock, "Agent 0");

        if (target != null)
            _cameraOffset = transform.position - target.transform.position;
    }

    void Update()
    {

        if (target != null)
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

        Quaternion squadViewOrientation = Quaternion.Euler(60, 0, 0);
        Quaternion strategicViewOrientation = Quaternion.Euler(90, 0, 0);

        // Strategic view
        if (Camera.main.transform.position.y >= maxHeight)
        {
            LookAtPlayer = false;
            Camera.main.orthographic = true;
            Camera.main.transform.position = new Vector3(OrthoOffset.x, maxHeight, OrthoOffset.z);
            Camera.main.transform.localRotation = strategicViewOrientation;
            Camera.main.orthographicSize = orthoSize;
        }

        // Squad View
        if (Camera.main.transform.position.y < maxHeight)
        {
            LookAtPlayer = true;
            Camera.main.transform.localRotation = squadViewOrientation;
            Camera.main.orthographic = false;

            Vector3 newPos = new Vector3((target.transform.position + _cameraOffset).x, Camera.main.transform.position.y, (target.transform.position + _cameraOffset).z);

            transform.position = Vector3.Slerp(transform.position, newPos, 1.0f);

            if (LookAtPlayer)
                transform.LookAt(target.transform);
        }

        }
        else
        {
            Start();
        }
    }

    GameObject GetChildWithName(GameObject obj, string name)
    {
        // Source: http://answers.unity.com/answers/1355797/view.html

        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }
}
