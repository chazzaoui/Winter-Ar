using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FurniturePlacer : MonoBehaviour
{
    public Transform placementIndicator;
 

    private List<GameObject> furniture = new List<GameObject>();
    private GameObject curSelected;
    private Camera cam;

    void Start ()
    {
        cam = Camera.main;
    }

    void Update () 
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = cam.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

          if(Physics.Raycast(ray, out hit))
            {   
    if(hit.collider.gameObject != null && furniture.Contains(hit.collider.gameObject))
                {
        if(curSelected != null && hit.collider.gameObject != curSelected)
            Select(curSelected);
        else if(curSelected == null)
            Select(hit.collider.gameObject);
                }
            }
        }
        if (curSelected != null &&  Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
        {
            MoveSelected();
        }
    }

    void Select (GameObject selected)
    {
        if (curSelected != null)
        ToggleSelectionVisual(curSelected, false);

        curSelected = selected;
        ToggleSelectionVisual(curSelected, true);
       
    }

    void MoveSelected ()
    {
        Vector3 curPos = cam.ScreenToViewportPoint(Input.touches[0].position);
        Vector3 lastPos= cam.ScreenToViewportPoint(Input.touches[0].position - Input.touches[0].deltaPosition);

        Vector3 touchDir = curPos - lastPos;

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        curSelected.transform.position += (camRight * touchDir.x + camForward * touchDir.y  );
    }

    void Deselect ()
    {
        if (curSelected != null)
        ToggleSelectionVisual(curSelected, false);

        curSelected = null;
      
    }

    void ToggleSelectionVisual (GameObject obj, bool toggle)
    {
        obj.transform.Find("Selected").gameObject.SetActive(toggle);
    }

    public void PlaceFurniture (GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, placementIndicator.position, Quaternion.identity);
        furniture.Add(obj);
        Select(obj);
    }
}
