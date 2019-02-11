using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraImage : MonoBehaviour {

    public CameraController cameraController;
    public ObjectManager objectManager;

    public void MoveCameraImageToObject()
    {
        //Change focused object layer so the camera won't see other object in the image
        objectManager.selectedGameObject.layer = 10;

        if (objectManager.selectedGameObject != null)
        {
            Debug.Log("Move Camera Image To Object : " + objectManager.selectedGameObject);
            StartCoroutine(MoveOverSeconds(objectManager.selectedGameObject, cameraController.moveToObjectRange, 0f));
        }
    }

    public IEnumerator MoveOverSeconds(GameObject targetObject, float range, float seconds)
    {
        float elapsedTime = 0;

        Vector3 startingPos = cameraController.transform.position;
        Vector3 startingRot = new Vector3(cameraController.transform.eulerAngles.x, cameraController.transform.eulerAngles.y, cameraController.transform.eulerAngles.z);

        transform.rotation = Quaternion.Euler(startingRot);

        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(startingPos, targetObject.transform.position - transform.forward * range, elapsedTime / seconds);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetObject.transform.position - transform.forward * range;
    }

}
