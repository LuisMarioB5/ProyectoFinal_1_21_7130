using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject creditsPanel;
    public GameObject instructionPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ShowInstructions ()
    {
        if (instructionPanel != null) instructionPanel.SetActive(true);
    }

    public void HideInstructions ()
    {
        if (instructionPanel != null) instructionPanel.SetActive(false);
    }

    public void ShowCredits ()
    {
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void HideCredits ()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
