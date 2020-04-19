using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [Header("Tooltip Elements")]
    public Text objectName;
    public Text objectValue;
    public Slider healthSlider;
    public Image fillImage;

    private Color _fullHealthColor = Color.green;
    private Color _zeroHealthColor = Color.red;

    private Destructible _destructible;

    private void OnEnable() {
        transform.position = Camera.main.WorldToScreenPoint(_destructible.anchorPoint.position);
        objectName.text = _destructible.objectName;
        objectValue.text = "$" + _destructible.objectValue.ToString("F2");

        SetHealthUI();
    }

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(_destructible.anchorPoint.position);
        SetHealthUI();
    }

    private void SetHealthUI() {
        float currentHealth = _destructible.GetCurrentHealth();
        float startingHealth = _destructible.health;

        healthSlider.value = _destructible.GetCurrentHealth() / startingHealth;
        fillImage.color = Color.Lerp(_zeroHealthColor, _fullHealthColor, currentHealth / startingHealth);
    }

    public void SetTarget(Destructible selected) {
        _destructible = selected;
    }
}
