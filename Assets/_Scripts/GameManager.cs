using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Panel Settings")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    [Header("UI References")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI centerText;
    public TextMeshProUGUI enemiesText;
    public Slider healthBar;

    public int enemiesAlive = 0;

    public void ShowAdWave(int waveNumber, int totalWaves)
    {
        if (centerText != null)
        {
            centerText.text = "WAVE " + waveNumber + "!";
            centerText.gameObject.SetActive(true);
            Invoke("HideCenterText", 3f);
        }

        if (waveText != null) waveText.text = "Wave: " + waveNumber + " / " + totalWaves;
    }

    void HideCenterText()
    {
        if (centerText != null) centerText.gameObject.SetActive(false);
    }

    public void UpdateEnemiesCounter (int amountSum)
    {
        enemiesAlive += amountSum;
        if (enemiesText != null) enemiesText.text = "Enemies: " + enemiesAlive;
    }

    public void UpdateHealthBar (float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;

            float healthPercent = currentHealth / maxHealth;
            Image fillImage = healthBar.fillRect.GetComponent<Image>();

            if (fillImage != null)
            {
                Color mixedColor = Color.Lerp(Color.red, Color.green, healthPercent);
                mixedColor.a = 1f;
                fillImage.color = mixedColor;
            }
        }
    }

    public void GameOver()
    {
        DeactivePlayer();
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Victory()
    {
        DeactivePlayer();
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void DeactivePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.GetComponent<PlayerController>().enabled = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
