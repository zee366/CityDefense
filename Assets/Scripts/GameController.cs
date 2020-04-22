using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    private const float MAX_CITY_DAMAGE = 12700f;
    private float currentCityDamage = 0f;

    private float gameTimer = 600f;
    private bool gameStarted = false;

    void Awake()
    {
        // singleton
        if(GameController.gameController == null) {
            GameController.gameController = this;
        }
        else {
            if(GameController.gameController != this) {
                Destroy(GameController.gameController.gameObject);
                GameController.gameController = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        if(gameStarted) {
            gameTimer -= Time.deltaTime;
            if(gameTimer < 0f || currentCityDamage > MAX_CITY_DAMAGE) {
                LoadScene("ClosingScene");
            }
        }
    }

    public void StartGame() {
        gameStarted = true;
    }

    void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }

    public void TakeDamage(float amount) {
        currentCityDamage += amount;
    }
}
