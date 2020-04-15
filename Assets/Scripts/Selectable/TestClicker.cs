using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestClicker : MonoBehaviour
{
    Selectable selected;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            if(selected)
                selected.ClearSelection();

            RaycastHit rayHit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit)) {
                Selectable obj = rayHit.collider.GetComponent<Selectable>();
                selected = obj ? obj.OnClick() : null;
            }
        }
    }
}
