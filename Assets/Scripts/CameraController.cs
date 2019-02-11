using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

    public ObjectManager objectManager;

    [Header("Normal Camera")]
    [Range(1f, 10f)]
    public float rotateSpeed = 5f;
    [Range(1f, 10f)]
    public float panSpeed = 10f;
    [Range(0.1f, 1f)]
    public float zoomSpeed = 1f;

    [Space]
    [Header("Orbit Camera")]
    [Range(1f, 10f)]
    public float orbitSpeed = 5f;
    [Range(1f, 20f)]
    public float nullRange = 10f;

    float distance;

    [Space]
    [Header("Camera Boundaries")]
    public Vector3 boundsMin = new Vector3(-10, -10, -10);
    public Vector3 boundsMax = new Vector3(10, 10, 10);

    //Private variables
    internal bool rotateCamera;
    internal bool panCamera;
    internal bool orbitCamera;

    internal float rotationSpeed = 5f;

    internal float moveToObjectRange = 2f;

    float rotationX;
    float rotationY;

    void Update()
    {
        //Shooting raycast from ScreenPoint
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
                    return;

                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(t.position);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Object"))
                    {
                        //Do something with the object that was hit by the raycast.
                        Debug.Log("Raycast hit : " + hit.transform.name);
                        Debug.Log("Selected GameObject : " + hit.transform.name);
                        objectManager.selectedGameObject = hit.transform.gameObject;

                        //Dectivate outline on gameObject that isn't selected
                        foreach (ObjectScript obj in objectManager.objectScripts)
                        {
                            if (obj.gameObject != objectManager.selectedGameObject)
                            {
                                obj.outline.enabled = false;
                            }
                            else
                            {
                                obj.outline.enabled = true;
                            }
                        }

                    }
                }
                else
                {
                    objectManager.selectedGameObject = null;

                    //Dectivate outline on gameObject that isn't selected
                    foreach (ObjectScript obj in objectManager.objectScripts)
                    {
                        obj.outline.enabled = false;
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        //Android
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            //Rotate camera

            if (!IsPointerOverUI(t))
            {
                if (rotateCamera)
                {
                    float newRotationX = transform.localEulerAngles.x - t.deltaPosition.y * rotateSpeed / 100f;
                    float newRotationY = transform.localEulerAngles.y + t.deltaPosition.x * rotateSpeed / 100f;

                    Quaternion rotationQ = Quaternion.Euler(newRotationX, newRotationY, 0f);
                    transform.rotation = rotationQ;
                }

                //Pan camera
                if (panCamera)
                {
                    transform.position += -transform.up * panSpeed / 10f * t.deltaPosition.y * Time.deltaTime;
                    transform.position += -transform.right * panSpeed / 10f * t.deltaPosition.x * Time.deltaTime;
                }

                //Orbit camera
                if (orbitCamera)
                {
                    RaycastHit hit;
                    var ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

                    if (Physics.Raycast(ray, out hit))
                    {
                        float newRotationX = transform.localEulerAngles.x - t.deltaPosition.y * rotateSpeed / 100f;
                        float newRotationY = transform.localEulerAngles.y + t.deltaPosition.x * rotateSpeed / 100f;

                        Quaternion rotation = Quaternion.Euler(newRotationX, newRotationY, 0);

                        distance = Vector3.Distance(transform.position, hit.point);

                        Vector3 position = hit.point - (rotation * Vector3.forward * distance);

                        transform.rotation = rotation;
                        transform.position = position;
                    }
                    else
                    {
                        float newRotationX = transform.localEulerAngles.x - t.deltaPosition.y * rotateSpeed / 100f;
                        float newRotationY = transform.localEulerAngles.y + t.deltaPosition.x * rotateSpeed / 100f;

                        Quaternion rotation = Quaternion.Euler(newRotationX, newRotationY, 0);

                        distance = Vector3.Distance(transform.position, transform.position + transform.forward * nullRange);

                        Vector3 position = transform.position + transform.forward * nullRange - (rotation * Vector3.forward * distance);

                        transform.rotation = rotation;
                        transform.position = position;
                    }
                }
            }
        }

        //Pinch to zoom camera
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (!IsPointerOverUI(touchZero) && !IsPointerOverUI(touchOne))
            {
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                transform.position += transform.forward * -deltaMagnitudeDiff * zoomSpeed / 10;
            }
        }

        //Set camera boundaries
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundsMin.x, boundsMax.x),
                                 Mathf.Clamp(transform.position.y, boundsMin.y, boundsMax.y),
                                 Mathf.Clamp(transform.position.z, boundsMin.z, boundsMax.z));
    }


    bool IsPointerOverUI(Touch touch)
    {
        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void MoveCameraToObject()
    {
        if (objectManager.selectedGameObject != null)
        {
            Debug.Log("Move Camera To Object : " + objectManager.selectedGameObject);
            StartCoroutine(MoveOverSeconds(objectManager.selectedGameObject, moveToObjectRange, 0.5f));
        }
    }

    public IEnumerator MoveOverSeconds(GameObject targetObject, float range, float seconds)
    {
        float elapsedTime = 0;

        Vector3 startingPos = transform.position;

        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(startingPos, targetObject.transform.position - transform.forward * range, elapsedTime / seconds);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetObject.transform.position - transform.forward * range;
    }

}
