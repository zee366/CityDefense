using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public SceneLoader sceneLoader;
    public Text timerText;
    public Slider cityHealthSlider;
    public Image cityHealthFillImage;
    public GameObject escapeMenu;

    private Color fullHealthColor = Color.green;
    private Color zeroHealthColor = Color.red;

    private const float MAX_CITY_HEALTH = 12700f;
    private float currentCityDamage = 0f;

    private float gameTimer = 600f;
    private bool gameRunning = false;

    public bool gamePaused = false;

    void Awake()
    {
        // singleton
        if(GameController.instance == null) {
            GameController.instance = this;
        }
        else {
            if(GameController.instance != this) {
                Destroy(GameController.instance.gameObject);
                GameController.instance = this;
            }
        }
        //DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        if(!gamePaused) {
            ProcessGameTime();
            ProcessCityHealth();
        }
    }

    public void DamageCity(float amount) {
        currentCityDamage += amount;
    }

    private void ProcessGameTime() {
        gameTimer -= Time.deltaTime;
        if(gameTimer < 0f) {
            sceneLoader.LoadScene("ClosingScene");
        }
        timerText.text = System.TimeSpan.FromSeconds((double)gameTimer).ToString(@"mm\:ss");
    }

    private void ProcessCityHealth() {
        cityHealthSlider.value = MAX_CITY_HEALTH - currentCityDamage;
        cityHealthFillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, cityHealthSlider.value / MAX_CITY_HEALTH);
        if(currentCityDamage > MAX_CITY_HEALTH) {
            sceneLoader.LoadScene("ClosingScene");
        }
    }

    public void PauseGame() {
        gamePaused = !gamePaused;
        if(gamePaused) {
            Time.timeScale = 0;
            escapeMenu.SetActive(true);
        }
        else {
            Time.timeScale = 1;
            escapeMenu.SetActive(false);
        }
    }
}
