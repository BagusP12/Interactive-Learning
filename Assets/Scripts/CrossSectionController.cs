using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossSectionController : MonoBehaviour {

    public GameObject crossSection;

    public Slider positionX;
    public Slider positionY;
    public Slider positionZ;

    public Slider rotationX;
    public Slider rotationY;
    public Slider rotationZ;

    Vector3 newPosition;
    Vector3 newRotation;

    public void ResetCrossSection()
    {
        positionX.value = 0;
        positionY.value = 0;
        positionZ.value = 0;

        rotationX.value = 0;
        rotationY.value = 0;
        rotationZ.value = 0;
    }

    public void UpdateValue(Vector3 position, Vector3 rotation)
    {
        Debug.Log("New CrossSection position : " + position + " | " + "rotation : " + rotation);
        newPosition = position;
        newRotation = rotation;
    }

    public void UpdateOnPointerUp()
    {
        UpdateValue(crossSection.transform.position, new Vector3(crossSection.transform.eulerAngles.x, crossSection.transform.eulerAngles.y, crossSection.transform.eulerAngles.z));
    }

    public void PositionX(float positionX)
    {
        crossSection.transform.position = new Vector3(newPosition.x + positionX, newPosition.y, newPosition.z);
    }

    public void PositionY(float positionY)
    {
        crossSection.transform.position = new Vector3(newPosition.x, newPosition.y + positionY, newPosition.z);
    }

    public void PositionZ(float positionZ)
    {
        crossSection.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z + positionZ);
    }

    public void RotateX(float rotationXAxis)
    {
        crossSection.transform.rotation = Quaternion.Euler(newRotation.x + rotationXAxis, newRotation.y, newRotation.z);
    }

    public void RotateY(float rotationYAxis)
    {
        crossSection.transform.rotation = Quaternion.Euler(newRotation.x, newRotation.y + rotationYAxis, newRotation.z);
    }

    public void RotateZ(float rotationZAxis)
    {
        crossSection.transform.rotation = Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z + rotationZAxis);
    }

}
