using UnityEngine;
using UnityEngine.EventSystems;

public class ClickBlock : MonoBehaviour
{
    // The camera we want to raycast through
    public Camera curCam;

    void OnGUI()
    {
        // Helpers to center GUI
        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(Screen.height));
        GUILayout.FlexibleSpace();

        GUILayout.Button("Demo Button", GUILayout.Width(100), GUILayout.Height(50));
        GUILayout.Space(100);

        // Helpers to center GUI
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void Update()
    {
        // Check for Canvas GUI
        bool blockedByCanvasUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        // Check for IMGUI
        bool blockedByIMGUI = GUIUtility.hotControl != 0;

        // Note: Only checking MouseButtonDown here.
        //       MouseButtonUp for IMGUI blocking would require checking last frame's GUIUtility.hotControl.
        if (Input.GetMouseButtonDown(0))
        {
            Collider colliderOver = RaycastFirstCollider(curCam);
            if (colliderOver == null)
                return;

            if (blockedByCanvasUI)
                Debug.Log("Click to " + colliderOver.name + " Blocked by Canvas UI!");
            else if (blockedByIMGUI)
                Debug.Log("Click to " + colliderOver.name + " Blocked by IMGUI UI!");
            else
                Debug.Log("Click to " + colliderOver.name + " Success!");
        }
    }

    // Raycast the current mouse pointer on a given camera. Returns first collider hit by distance
    Collider RaycastFirstCollider(Camera cam)
    {
        if (cam == null)
            return null;

        Ray curRay = cam.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(curRay, cam.farClipPlane);

        if (hits.Length < 1)
            return null;

        // Ensure that hits are in order of shortest to longest (not guaranteed by default!)
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        return hits[0].collider;
    }
}