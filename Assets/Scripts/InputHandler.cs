using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public UIPoliceAbilities abilityBar;
    public CameraController cameraController;
    public Tooltip tooltip;
    private Destructible _selected;

    void Update()
    {
        CheckInput();
    }

    void CheckInput() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            GameController.instance.PauseGame();
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            abilityBar.OnArrestButtonClicked();
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            abilityBar.OnAggressiveArrestButtonClicked();
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            abilityBar.OnRubberBulletsButtonClicked();
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)) {
            abilityBar.OnFireBulletsButtonClicked();
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha5)) {
            abilityBar.OnSmokeGrenadeButtonClicked();
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)) {
            abilityBar.OnWaterCannonButtonClicked();
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha7)) {
            abilityBar.OnLethalBulletsButtonClicked();
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha8)) {
            abilityBar.OnReinforceSquadButtonClicked();
            return;
        }
        if(Input.GetKeyDown(KeyCode.C)) {
            // FormCircle()
        }
        if(Input.GetKeyUp(KeyCode.C)) {
            // for each flock agent?
            // navMeshAgent.destination = transform.position
        }
        if(Input.GetKey(KeyCode.LeftShift)) {
            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                // FormLineFacingLeft()
                return;
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                // FormLineFacingRight()
                return;
            }
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                // FormLineFacingForward()
                return;
            }
            if(Input.GetKeyDown(KeyCode.DownArrow)) {
                // FormLineFacingBackward()
                return;
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            // FaceLeft()
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            // FaceRight()
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            // FaceForward()
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            // FaceDown()
        }
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
        if(Input.GetKey(KeyCode.Mouse2)) {
            cameraController.OrbitDrag(Input.GetAxis("Mouse X"));
        }
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraController.ScrollToZoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }
}
