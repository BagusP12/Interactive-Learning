using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using cakeslice;

[RequireComponent(typeof(Outline), typeof(CuttingController))]
public class ObjectScript : MonoBehaviour {

    public AudioClip audioVoice;

    [TextArea]
    public string objectDescription;

    internal bool rotateObject;
    internal bool dragObject;

    private float duration = 1f;
    private Vector3 objStartPosition;
    private Vector3 objStartRotation;
    private Vector3 objStartScale;

    private Vector3 objCurPosition;
    private Vector3 objCurRotation;
    private Vector3 objCurScale;

    Camera camera;

    internal Outline outline;

    internal ObjectManager objectManager;

    internal float rotationSpeed = 2.5f;

    Vector3 screenPoint;
    Vector3 offset;

    LabelLineRenderer labelLineRenderer;

    void Start () {
        camera = Camera.main;

        objStartPosition = gameObject.transform.position;
        objStartRotation = new Vector3(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z);
        objStartScale = gameObject.transform.localScale;

        outline = GetComponent<Outline>();
        outline.enabled = false;

        if (transform.GetComponentInChildren<LabelLineRenderer>() != null)
        {
            labelLineRenderer = transform.GetComponentInChildren<LabelLineRenderer>();
        }
    }

    //private void OnMouseEnter()
    //{
    //    if (dragObject || rotateObject)
    //    {
    //        outline.enabled = true;
    //    }
    //}

    private void OnMouseDown()
    {
        //Backup
        //screenPoint = camera.WorldToScreenPoint(gameObject.transform.position);
        //offset = gameObject.transform.position - camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            screenPoint = camera.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - camera.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, screenPoint.z));
        }
    }

    private void OnMouseDrag()
    {

        ////Backup
        ////Drag object
        //if (dragObject)
        //{
        //    Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        //    Vector3 objectPosition = camera.ScreenToWorldPoint(mousePosition) + offset;
        //    transform.position = objectPosition;
        //}
        ////Rotate object
        //if (rotateObject)
        //{
        //    float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
        //    float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;

        //    transform.RotateAround(camera.transform.up, -rotationX);
        //    transform.RotateAround(camera.transform.right, rotationY);

        //    if (Input.touchCount == 1)
        //    {
        //        Touch t = Input.GetTouch(0);

        //        if (!IsPointerOverUI(t))
        //        {
        //            rotationX = t.deltaPosition.x * rotationSpeed * Mathf.Deg2Rad / 10f;
        //            rotationY = t.deltaPosition.y * rotationSpeed * Mathf.Deg2Rad / 10f;

        //            transform.RotateAround(camera.transform.up, -rotationX);
        //            transform.RotateAround(camera.transform.right, rotationY);
        //        }
        //    }
        //}


        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (!IsPointerOverUI(t))
            {
                //Drag object
                if (dragObject)
                {
                    Vector3 touchPosition = new Vector3(t.position.x, t.position.y, screenPoint.z);
                    Vector3 objectPosition = camera.ScreenToWorldPoint(touchPosition) + offset;
                    transform.position = objectPosition;
                }

                //Rotate object
                if (rotateObject)
                {

                    float rotationX = t.deltaPosition.x * rotationSpeed * Mathf.Deg2Rad / 10f;
                    float rotationY = t.deltaPosition.y * rotationSpeed * Mathf.Deg2Rad / 10f;

                    transform.RotateAround(camera.transform.up, -rotationX);
                    transform.RotateAround(camera.transform.right, rotationY);
                }
            }
        }

        //Updating label position and rotation;
        if (labelLineRenderer != null)
        {
            labelLineRenderer.UpdateLineRenderer();
        }
    }

    //private void OnMouseExit()
    //{
    //    //outline.enabled = false;
    //}

    bool IsPointerOverUI(Touch touch)
    {
        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    public void ResetPosition()
    {
        objCurPosition = gameObject.transform.position;
        objCurRotation = new Vector3(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z);
        objCurScale = gameObject.transform.localScale;
        if (objCurPosition != objStartPosition || objCurRotation != objStartRotation || objCurScale != objStartScale)
        {
            StartCoroutine(ResetOverSeconds(gameObject, objStartPosition, objCurRotation, objStartRotation, objCurScale, objStartScale, duration));
        }
    }

    public IEnumerator ResetOverSeconds(GameObject objectToMove, Vector3 destination, Vector3 rotateFrom, Vector3 rotateTo, Vector3 scaleFrom, Vector3 scaleTo, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, destination, elapsedTime / seconds);

            objectToMove.transform.rotation = Quaternion.Slerp(Quaternion.Euler(rotateFrom), Quaternion.Euler(rotateTo), elapsedTime / seconds);

            objectToMove.transform.localScale = Vector3.Lerp(scaleFrom, scaleTo, elapsedTime / seconds);

            //Updating label position and rotation;
            if (labelLineRenderer != null)
            {
                labelLineRenderer.UpdateLineRenderer();
            }

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = destination;
        objectToMove.transform.rotation = Quaternion.Euler(rotateTo);
        objectToMove.transform.localScale = scaleTo;
    }

}
