using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePanel;
    public GameObject menuPanel;
    public GameManager gameManager;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }
    public void OnAnyButtonClick()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.clickSFX);
    }
    public void PauseGame()
    {
        OnAnyButtonClick();
        AudioManager.instance.PlayMusic(
            AudioManager.instance.pauseMusic,
            AudioManager.instance.pauseGroup
        );
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        OnAnyButtonClick();
        AudioManager.instance.PlayMusic(
            AudioManager.instance.gameMusic,
            AudioManager.instance.gameGroup
        );
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        OnAnyButtonClick();
        Time.timeScale = 0f;
        menuPanel.SetActive(true);
        pausePanel.SetActive(false);
    }
    public void Restart()
    {
        OnAnyButtonClick();
        AudioManager.instance.PlayMusic(
            AudioManager.instance.gameMusic,
            AudioManager.instance.gameGroup
        );
        pausePanel.SetActive(false);
        gameManager.ResetGame();
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void QuitGame()
    {
        OnAnyButtonClick();
        Debug.Log("IZHOD");
        Application.Quit();
    }
}
