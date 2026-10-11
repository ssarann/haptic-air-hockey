using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerScore = 0;
    public int opponentScore = 0;
    public int maxScore = 5;

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI opponentScoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI playerScoreText2;
    public TextMeshProUGUI opponentScoreText2;
    public TextMeshProUGUI timerText2;
    public TextMeshProUGUI playerScoreText3;
    public TextMeshProUGUI opponentScoreText3;
    public TextMeshProUGUI timerText3;

    private GameObject UDP;
    private GameUDPServer UDPScript;
    public float resetPuckOnGoal = 0;

    private float roundTime = 0f;

    public GameObject puck;
    public Transform puckStartPosition;


    public GameObject goalPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject winPanel1;
    public GameObject winPanel2;
    public GameObject NasVuf;

    void Start()
    {
        UDP = GameObject.Find("UDP");
        if (GameObject.Find("UDP"))
        {
            UDP = GameObject.Find("UDP");
            UDPScript = UDP.GetComponent<GameUDPServer>();
            print("UDP GameObject found");
        }
        resetPuckOnGoal = 1;
        UDPScript.sendFloat3(resetPuckOnGoal, 0f, 0f);
        UpdateScoreUI();
        roundTime = 0f;
        ResetPuck();
    }

    void Update()
    {
        // stetje časa
        roundTime += Time.deltaTime;
        timerText.text = FormatTime(roundTime);
        timerText2.text = FormatTime(roundTime);
        timerText3.text = FormatTime(roundTime);
    }

    // Formatiranje casa (mm:ss:ms)
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        //int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Klic, ko je dosežen gol
    public void GoalScored(bool isPlayerGoal)
    {
        if (isPlayerGoal)
        {
            playerScore++;
            AudioManager.instance.PlaySFX(AudioManager.instance.goalSFX);
        }
        else
        {
            opponentScore++;
            AudioManager.instance.PlaySFX(AudioManager.instance.goalSFX);
        }
        UpdateScoreUI();

     
        if (playerScore >= maxScore)
        {
            ResetGame();
            AudioManager.instance.PlaySFX(AudioManager.instance.wonSFX);
            if (NasVuf.activeSelf)
            {
                StartCoroutine(ShowPanelAndPause(winPanel1));
            }
            else
            {
                StartCoroutine(ShowPanelAndPause(winPanel));
            }
            
        }
        else if (opponentScore >= maxScore)
        {
            ResetGame();
            
            if (NasVuf.activeSelf)
            {
                StartCoroutine(ShowPanelAndPause(winPanel2));
                AudioManager.instance.PlaySFX(AudioManager.instance.wonSFX);
            }
            else
            {
                StartCoroutine(ShowPanelAndPause(losePanel));
                AudioManager.instance.PlaySFX(AudioManager.instance.lostSFX);
            }
            
        }
        else 
        {
            ResetPuck();
            StartCoroutine(ShowPanelAndPause(goalPanel));
        }
    }

    void UpdateScoreUI()
    {
        if (playerScoreText != null)
        {
            playerScoreText.text = "" + playerScore;
            playerScoreText2.text = "" + playerScore;
            playerScoreText3.text = "" + playerScore;
        }
        if (opponentScoreText != null)
        {
            opponentScoreText.text = "" + opponentScore;
            opponentScoreText2.text = "" + opponentScore;
            opponentScoreText3.text = "" + opponentScore;
        }
    }

    void ResetPuck()
    {
        if (puck != null)
        {
            resetPuckOnGoal = 1;
            UDPScript.sendFloat3(resetPuckOnGoal, 0f, 0f);

            StartCoroutine(ResetFlagOffDelay(0.5f)); // 0.5s zakasnitev
        }
    }
    private IEnumerator ShowPanelAndPause(GameObject panel)
    {
        // pokaži panel in ustavi igro
        panel.SetActive(true);
        Time.timeScale = 0f;

        // počakaj 2 sekundi v realnem času
        yield return new WaitForSecondsRealtime(2f);

        // spelji igro naprej in skrij panel
        Time.timeScale = 1f;
        panel.SetActive(false);
    }
    IEnumerator ResetFlagOffDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        resetPuckOnGoal = 0;
        //UDPScript.sendFloat3(resetPuckOnGoal, 0f, 0f);
    }

    // Resetiraj igro
    public void ResetGame()
    {
        playerScore = 0;
        opponentScore = 0;
        roundTime = 0f;
        UpdateScoreUI();
        ResetPuck();
    }
}
