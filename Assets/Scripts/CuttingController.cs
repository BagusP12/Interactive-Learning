using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class CuttingController : MonoBehaviour {

    public GameObject crossSection;
    Vector3 normal;
    Vector3 position;
    Material mat;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        normal = crossSection.transform.TransformVector(new Vector3(0, 0, -1));
        position = crossSection.transform.position;
        UpdateShaderProperties();
    }
    void Update()
    {
        UpdateShaderProperties();
    }

    private void UpdateShaderProperties()
    {
        normal = crossSection.transform.TransformVector(new Vector3(0, 0, -1));
        position = crossSection.transform.position;
        rend.material.SetVector("_PlaneNormal", normal);
        rend.material.SetVector("_PlanePosition", position);
    }

}
