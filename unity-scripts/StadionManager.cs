using UnityEngine;

public class StadionManager : MonoBehaviour
{
    [Header("Arene")]
    public GameObject[] arenas;

    [Header("Kamere")]
    public GameObject[] cameras;

    [Header("Game-mode: PvAI ali PvP")]
    public GameObject ARCamera;
    public GameObject ImageTarget;
    public GameObject NasprotnikAI;
    public GameObject Nasprotnik;

    public GameObject MejaSplitScreen;
    [Header("Nastavitve AI")]
    public OpponentAI opponentAI;

    void Start()
    {
        // pri prvem zagonu scene
        ApplySettings();
    }

    /// <summary>
    /// Pokliče MainMenu takoj po izbiri "Igraj", ali pa tudi v Start().
    /// </summary>
    public void ApplySettings()
    {
        // 1) GameMode
        bool isPvAI = PlayerPrefs.GetInt("GameMode", 1) == 1;
        ARCamera.SetActive(!isPvAI);
        ImageTarget.SetActive(!isPvAI);
        Nasprotnik.SetActive(!isPvAI);
        NasprotnikAI.SetActive(isPvAI);

        // 2) Arena
        int selArena = PlayerPrefs.GetInt("SelectedArena", 0);
        for (int i = 0; i < arenas.Length; i++)
            arenas[i].SetActive(i == selArena);

        // 3) Kamera
        //int selCam = PlayerPrefs.GetInt("SelectedCamera", 0);
        //for (int i = 0; i < cameras.Length; i++)
        //    cameras[i].SetActive(i == selCam);

        // 4) Težavnost AI
        int diff = PlayerPrefs.GetInt("SelectedDifficulty", 1);
        opponentAI.SetDifficulty(diff);


        // Kamera
        int selCam = PlayerPrefs.GetInt("SelectedCamera", 0);
        if (isPvAI)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(i == selCam);
                MejaSplitScreen.SetActive(false);
            }


        }
        else if (!isPvAI)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(i == (selCam + 2) || i == (selCam + 4));

                MejaSplitScreen.SetActive(true);
            }
        }
    }
}
