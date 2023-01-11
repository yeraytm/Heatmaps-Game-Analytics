using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    Transform cameraTransform;
    Transform pivotTransform;

    Vector3 localRotation;

    float cameraDistance = 20f;

    public float mouseSens = 4f;
    public float scrollSens = 2f;
    public float orbitDampening = 10f;
    public float scrollDampening = 6f;

    public bool cameraDisabled = false;

    void Start()
    {
        cameraTransform = transform;
        pivotTransform = transform.parent;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            cameraDisabled = !cameraDisabled;

        if (!cameraDisabled)
        {
            if (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f)
            {
                localRotation.x += Input.GetAxis("Mouse X") * mouseSens;
                localRotation.y -= Input.GetAxis("Mouse Y") * mouseSens;

                localRotation.y = Mathf.Clamp(localRotation.y, 0f, 90f);
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float scrollValue = Input.GetAxis("Mouse ScrollWheel") * scrollSens;
                scrollValue *= cameraDistance * 0.3f;

                cameraDistance += scrollValue * -1f;

                cameraDistance = Mathf.Clamp(cameraDistance, 1.5f, 100f);
            }
        }

        Quaternion quat = Quaternion.Euler(localRotation.y, localRotation.x, 0);
        pivotTransform.rotation = Quaternion.Lerp(pivotTransform.rotation, quat, Time.deltaTime * orbitDampening);

        if (cameraTransform.localPosition.z != cameraDistance * -1f)
        {
            cameraTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(
                cameraTransform.localPosition.z,
                cameraDistance * -1f,
                Time.deltaTime * scrollDampening));
        }
    }
}