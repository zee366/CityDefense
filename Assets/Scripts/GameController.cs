using System.Collections;
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
    public UIFader fadeToBlackPanel;

    private Color fullHealthColor = Color.green;
    private Color zeroHealthColor = Color.red;

    private const float MAX_CITY_HEALTH = 12700f;
    private float currentCityDamage = 0f;

    public float gameTimer;
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
        gameRunning = true;
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
        if(gameRunning) {
            gameTimer -= Time.deltaTime;
            if(gameTimer < 0f) {
                gameRunning = false;
                StartCoroutine(EndGame(fadeToBlackPanel.fadeDuration));
                fadeToBlackPanel.StartFade();
            }
            timerText.text = System.TimeSpan.FromSeconds((double)gameTimer).ToString(@"mm\:ss");
        }
    }

    IEnumerator EndGame(float time) {
        yield return new WaitForSeconds(time);
        sceneLoader.LoadScene("ClosingScene");
        yield return null;
    }

    private void ProcessCityHealth() {
        if(gameRunning) {
            cityHealthSlider.value = MAX_CITY_HEALTH - currentCityDamage;
            cityHealthFillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, cityHealthSlider.value / MAX_CITY_HEALTH);
            if(currentCityDamage > MAX_CITY_HEALTH) {
                gameRunning = false;
                StartCoroutine(EndGame(fadeToBlackPanel.fadeDuration));
                fadeToBlackPanel.StartFade();
            }
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
