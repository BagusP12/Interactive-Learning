using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectManager : MonoBehaviour
{

    public SceneManager sceneManager;
    public CameraController cameraController;
    public CrossSectionController crossSectionController;

    public GameObject crossSectionObject;
    public GameObject crossSectionPanel;

    internal GameObject selectedGameObject;

    [Header("List")]
    internal ObjectScript[] objectScripts;

    GameObject[] labelGameObject;

    bool hideOtherObject;

    void Start()
    {
        if (FindObjectsOfType<ObjectScript>() != null)
        {
            objectScripts = new ObjectScript[FindObjectsOfType<ObjectScript>().Length];
            objectScripts = FindObjectsOfType<ObjectScript>();
        }

        foreach (ObjectScript objScript in objectScripts)
        {
            objScript.objectManager = this;
        }

        labelGameObject = GameObject.FindGameObjectsWithTag("Label");

        ActivateOrbitCamera();
    }

    public void Reset()
    {
        UnhideOtherObject();

        foreach (ObjectScript objScript in objectScripts)
        {
            Debug.Log("Reset object position");
            objScript.ResetPosition();
        }
    }

    public void HideOtherObject()
    {
        Debug.Log("Hide/Unhide other object");

        hideOtherObject = !hideOtherObject;

        if (selectedGameObject)
        {
            foreach (ObjectScript obj in objectScripts)
            {
                if (hideOtherObject)
                {
                    if (obj.gameObject != selectedGameObject)
                    {
                        obj.gameObject.SetActive(false);
                    }
                }
                else
                {
                    obj.gameObject.SetActive(true);
                }
            }
        }
    }

    public void UnhideOtherObject()
    {
        if (hideOtherObject)
        {
            hideOtherObject = false;

            foreach (ObjectScript obj in objectScripts)
            {
                obj.gameObject.SetActive(true);
            }
        }
    }

    public void UnselectObject()
    {
        selectedGameObject = null;
    }

    public void ActivateLabel(bool activateLabel)
    {
        foreach (GameObject label in labelGameObject)
        {
            label.SetActive(activateLabel);
        }
    }

    public void ActivateRotateCamera()
    {
        Debug.Log("Rotate camera activate");

        cameraController.rotateCamera = true;
        cameraController.panCamera = false;
        cameraController.orbitCamera = false;

        foreach (ObjectScript objScript in objectScripts)
        {
            objScript.rotateObject = false;
            objScript.dragObject = false;
        }
    }

    public void ActivatePanCamera()
    {
        Debug.Log("Pan camera activate");

        cameraController.rotateCamera = false;
        cameraController.panCamera = true;
        cameraController.orbitCamera = false;

        foreach (ObjectScript objScript in objectScripts)
        {
            objScript.rotateObject = false;
            objScript.dragObject = false;
        }
    }

    public void ActivateOrbitCamera()
    {
        Debug.Log("Pan camera activate");

        cameraController.rotateCamera = false;
        cameraController.panCamera = false;
        cameraController.orbitCamera = true;

        foreach (ObjectScript objScript in objectScripts)
        {
            objScript.rotateObject = false;
            objScript.dragObject = false;
        }
    }

    public void ActivateRotateObject()
    {
        Debug.Log("Rotate object activate");

        cameraController.rotateCamera = false;
        cameraController.panCamera = false;
        cameraController.orbitCamera = false;

        foreach (ObjectScript objScript in objectScripts)
        {
            objScript.rotateObject = true;
            objScript.dragObject = false;
        }
    }

    public void ActivateDragObject()
    {
        Debug.Log("Drag object activate");

        cameraController.rotateCamera = false;
        cameraController.panCamera = false;
        cameraController.orbitCamera = false;

        foreach (ObjectScript objScript in objectScripts)
        {
            objScript.rotateObject = false;
            objScript.dragObject = true;
        }
    }

    public void CrossSection()
    {
        crossSectionObject.GetComponent<MeshRenderer>().enabled = !crossSectionObject.GetComponent<MeshRenderer>().enabled;
        crossSectionPanel.active = !crossSectionPanel.active;

        crossSectionController.ResetCrossSection();

        if (!crossSectionObject.GetComponent<MeshRenderer>().enabled)
        {
            crossSectionObject.transform.position = cameraController.transform.position - cameraController.transform.forward * 50;
            crossSectionObject.transform.rotation = cameraController.transform.rotation;
        }
        else
        {
            RaycastHit hit;

            if (Physics.SphereCast(cameraController.GetComponent<Camera>().transform.position, 2f, cameraController.GetComponent<Camera>().transform.forward, out hit))
            {
                Debug.Log("SphereCast cast hit : " + hit.transform.name);
                crossSectionObject.transform.position = hit.transform.position;
                crossSectionObject.transform.rotation = cameraController.transform.rotation;

                crossSectionController.UpdateValue(hit.transform.position, new Vector3(cameraController.transform.eulerAngles.x, cameraController.transform.eulerAngles.y, cameraController.transform.eulerAngles.z));
            }
            else
            {
                Debug.Log("SphereCast doesn't hit anything!");
                crossSectionObject.transform.position = cameraController.transform.position - cameraController.transform.forward * -5;
                crossSectionObject.transform.rotation = cameraController.transform.rotation;

                crossSectionController.UpdateValue(cameraController.transform.position - cameraController.transform.forward * -5, new Vector3(cameraController.transform.eulerAngles.x, cameraController.transform.eulerAngles.y, cameraController.transform.eulerAngles.z));
            }
        }
    }

}
