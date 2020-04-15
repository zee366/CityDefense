using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestClicker : MonoBehaviour
{
    public Tooltip tooltip;

    private Destructible _selected;

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            if(_selected) {
                tooltip.gameObject.SetActive(false);
            }

            RaycastHit rayHit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit)) {
                _selected = rayHit.collider.GetComponent<Destructible>();
                if(_selected) {
                    tooltip.SetTarget(_selected);
                    tooltip.gameObject.SetActive(true);
                }
            }
        }
    }
}
