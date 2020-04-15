using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour
{
    public bool Selected { get; set; }
    private Tooltip _tooltip;

    void Start()
    {
        _tooltip = GetComponentInChildren<Tooltip>();
        _tooltip.gameObject.SetActive(false);
    }

    public void ClearSelection() {
        Selected = false;
        _tooltip.gameObject.SetActive(false);
    }

    public Selectable OnClick() {
        Selected = true;
        _tooltip.gameObject.SetActive(true);
        return this;
    }
}
