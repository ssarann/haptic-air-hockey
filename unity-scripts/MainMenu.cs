using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject igraPanel;

    [Header("Izbira stadiona")]
    public TMP_Dropdown arenaDropdown;
    public string[] arenaNames;

    [Header("Izbira težavnosti")]
    public TMP_Dropdown difficultyDropdown;
    public string[] difficultyLabels = { "Lahek", "Srednji", "Težek" };

    [Header("Izbira kamere")]
    public TMP_Dropdown cameraDropdown;
    public string[] cameraNames;
    public Sprite[] previewSprites;
    public Image cameraPreviewImage;

    public GameManager gameManager;
    // referenca na manager, ki upravlja aktivacijo kamer, aren in AI/AR
    public StadionManager stadionManager;

    void Start()
    {
        // napolnimo dropdown-e
        arenaDropdown.ClearOptions();
        arenaDropdown.AddOptions(new System.Collections.Generic.List<string>(arenaNames));
        difficultyDropdown.ClearOptions();
        difficultyDropdown.AddOptions(new System.Collections.Generic.List<string>(difficultyLabels));
        cameraDropdown.ClearOptions();
        cameraDropdown.AddOptions(new System.Collections.Generic.List<string>(cameraNames));

        // event listenerji
        arenaDropdown.onValueChanged.AddListener(_ => UpdatePreview());
        cameraDropdown.onValueChanged.AddListener(_ => UpdatePreview());
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        AudioManager.instance.PlayMusic(
            AudioManager.instance.menuMusic,
            AudioManager.instance.menuGroup
        );
        Time.timeScale = 0f;
    }

    // tipki “Igraj”
    public void PlayVsPlayer()
    {
        PlayerPrefs.SetInt("GameMode", 0);
        PlayerPrefs.Save();
        OnAnyButtonClick();
        ApplyAllSettings();
        gameManager.ResetGame();
        AudioManager.instance.PlayMusic(
        AudioManager.instance.gameMusic,
        AudioManager.instance.gameGroup
    );
        Time.timeScale = 1f;
    }
    public void PlayVsAI()
    {
        PlayerPrefs.SetInt("GameMode", 1);
        PlayerPrefs.Save();
        OnAnyButtonClick();
        ApplyAllSettings();
        gameManager.ResetGame();
        AudioManager.instance.PlayMusic(
        AudioManager.instance.gameMusic,
        AudioManager.instance.gameGroup
    );
        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        OnAnyButtonClick();
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        OnAnyButtonClick();
        settingsPanel.SetActive(false);
    }

    public void OpenIgraPanel()
    {
        OnAnyButtonClick();
        ApplyAllSettings();
        UpdatePreview();
        menuPanel.SetActive(false);
        igraPanel.SetActive(true);
    }
    public void CloseIgraPanel()
    {
        OnAnyButtonClick();
        igraPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        OnAnyButtonClick();
        Application.Quit();
    }

    // pregeneriraj preview sprite
    private void UpdatePreview()
    {
        int cam = cameraDropdown.value;
        int arena = arenaDropdown.value;
        int idx = cam * arenaNames.Length + arena;
        if (idx >= 0 && idx < previewSprites.Length)
            cameraPreviewImage.sprite = previewSprites[idx];
    }

    // to kličeva takoj ko smo izbrali "Igraj"
    private void ApplyAllSettings()
    {
        // shrani PlayerPrefs
        PlayerPrefs.SetInt("SelectedArena", arenaDropdown.value);
        PlayerPrefs.SetInt("SelectedDifficulty", difficultyDropdown.value);
        PlayerPrefs.SetInt("SelectedCamera", cameraDropdown.value);
        PlayerPrefs.Save();

        // in pokliči StadionManager, da uredi vse
        stadionManager.ApplySettings();
        menuPanel.SetActive(false);
    }

    private void OnAnyButtonClick()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.clickSFX);
    }
}
