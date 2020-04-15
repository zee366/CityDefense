using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;
    public Transform AnchorPoint;

    private Color _fullHealthColor = Color.green;
    private Color _zeroHealthColor = Color.red;

    private Destructible _destructible;

    private void OnEnable() {
        _destructible = GetComponentInParent<Destructible>();
        transform.position = Camera.main.WorldToScreenPoint(AnchorPoint.position);
        SetHealthUI();
    }

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(AnchorPoint.position);
        SetHealthUI();
    }

    private void SetHealthUI() {
        float currentHealth = _destructible.GetCurrentHealth();
        float startingHealth = _destructible.health;

        slider.value = _destructible.GetCurrentHealth() / startingHealth;
        fillImage.color = Color.Lerp(_zeroHealthColor, _fullHealthColor, currentHealth / startingHealth);
    }
}
