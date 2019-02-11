using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelLineRenderer : MonoBehaviour {

    public GameObject parentObject;

    public float lineRendererWidth = 0.025f;

    public Color lineRendererColor = Color.white;

    public Color labelColor = Color.white;

    LineRenderer lineRenderer;

    TextMeshPro textMeshPro;

    Camera camera;

    void Start () {
        gameObject.layer = 8;

        parentObject = transform.parent.gameObject;
        camera = Camera.main;

        SetupLineRenderer();

        textMeshPro = GetComponent<TextMeshPro>();
        textMeshPro.color = labelColor;
        textMeshPro.text = parentObject.name;

        UpdateLineRenderer();
    }

    void SetupLineRenderer()
    {
        gameObject.AddComponent<LineRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = Resources.Load("Materials/LineRenderer") as Material;
        lineRenderer.SetWidth(lineRendererWidth, lineRendererWidth);
        lineRenderer.numCornerVertices = 90;
        lineRenderer.numCapVertices = 90;
        lineRenderer.SetColors(lineRendererColor, lineRendererColor);
    }

    public void UpdateLineRenderer()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, parentObject.transform.position);

        Quaternion m_Rotation;
        m_Rotation = Quaternion.Euler(camera.transform.rotation.x, camera.transform.rotation.y, 0f);
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, m_Rotation * Vector3.up);
    }

}
