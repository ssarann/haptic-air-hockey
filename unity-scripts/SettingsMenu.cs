using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Dropdown qualityDropdown;     // spustni meni za kakovost
    public TMP_Dropdown resolutionDropdown;  // spustni meni za rezolucijo
    public Slider volumeSlider;              // drsnik glasnosti
    public TMP_Text volumeValueLabel;        // tekst ob drsniku

    [Header("Audio")]
    public AudioMixer masterMixer;           // AudioMixer, assign v Inspectorju

    private Resolution[] resolutions;

    void Start()
    {
        // ─── 1) Naredimo svoj seznam kakovosti v slovenščini ───────────
        var kakovosti = new System.Collections.Generic.List<string>
        {
            "Najnizja",
            "Nizka",
            "Srednja",
            "Visoka",
            "Najvisja"
        };

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(kakovosti);

        // naložimo shranjeno vrednost ali privzeto iz QualitySettings
        int shranjena = PlayerPrefs.GetInt("QualityIndex", QualitySettings.GetQualityLevel());
        qualityDropdown.value = shranjena;
        QualitySettings.SetQualityLevel(shr​anjena);

        // povezava callbacka
        qualityDropdown.onValueChanged.RemoveAllListeners();
        qualityDropdown.onValueChanged.AddListener(SetQuality);


        // —– 2) Napolnimo seznam ločljivosti iz sistema ——
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var opts = new System.Collections.Generic.List<string>();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            var r = resolutions[i];

            // 1) izračunamo pravo Hz preko razmerja numerator/denominator
            float ratio = (float)r.refreshRateRatio.numerator / r.refreshRateRatio.denominator;
            int hz = Mathf.RoundToInt(ratio);

            // dodamo v seznam: "1920 × 1080 @ 60Hz"
            opts.Add($"{r.width} × {r.height} @ {hz}Hz");

            // preverimo, ali je to naša trenutna ločljivost + Hz
            var cr = Screen.currentResolution;
            float currRatio = (float)cr.refreshRateRatio.numerator / cr.refreshRateRatio.denominator;
            int currHz = Mathf.RoundToInt(currRatio);

            if (r.width == cr.width && r.height == cr.height && hz == currHz)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(opts);

        // preberemo, katero smo že shranili ali uporabimo trenutno
        int savedR = PlayerPrefs.GetInt("ResolutionIndex", currentIndex);
        resolutionDropdown.value = savedR;
        resolutionDropdown.RefreshShownValue();

        // vežemo callback
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);




        // ─── 3) Nastavimo drsnik glasnosti ────────────────────────────
        float shrVol = PlayerPrefs.GetFloat("MasterVol", 0.75f);
        volumeSlider.value = shrVol;

        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(SetVolume);

        SetVolume(shrVol);
    }

    // ─── Nastavi kakovost grafike in shrani ─────────────────────────
    public void SetQuality(int idx)
    {
        QualitySettings.SetQualityLevel(idx);
        PlayerPrefs.SetInt("QualityIndex", idx);
    }

    // ─── Nastavi rezolucijo in shrani ───────────────────────────────
    public void SetResolution(int idx)
    {
        var r = resolutions[idx];

        // uporabimo novo overload, ki sprejme RefreshRate struct
        Screen.SetResolution(
            r.width,
            r.height,
            Screen.fullScreenMode,
            r.refreshRateRatio
        );

        PlayerPrefs.SetInt("ResolutionIndex", idx);
        PlayerPrefs.Save();

        Debug.Log($"Nastavljena rezolucija: {r.width}×{r.height}@{Mathf.RoundToInt((float)r.refreshRateRatio.numerator / r.refreshRateRatio.denominator)}Hz");
    }


    // ─── Nastavi glasnost (drsnik 0–1 → decibeli) ───────────────────
    public void SetVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.001f, 1f)) * 20f;
        masterMixer.SetFloat("MasterVol", dB);

        volumeValueLabel.text = $"{Mathf.RoundToInt(sliderValue * 100f)}%";
        PlayerPrefs.SetFloat("MasterVol", sliderValue);
        PlayerPrefs.Save();
    }
}
