using UnityEngine;

public class OpponentAI : MonoBehaviour
{
    [Header("Reference")]
    public Transform puckTransform;
    public Transform nasTransform;
    public GameManager gameManager;

    [Header("Pozicijski pragovi (lokalno)")]
    public float centerLineX = -0.0559f;
    public float aiGoalX = -0.567f;
    public float defenseX = -0.5f;

    [Header("Gibanje in odziv")]
    public float movementSpeed = 3f;
    public float reactionDelay = 0.2f;
    private float reactionTimer = 0f;

    [Header("Ofenzivni parametri")]
    public float offensiveApproachDistance = 0.1f;
    public float offensiveYOffset = 0.0f;
    public float hitThresholdX = -0.4f;

    [Header("Defenzivni parametri")]
    public float defensiveYOffset = 0.0f;

    [Header("Difficulty")]
    [Tooltip("0 = Lahek, 1 = Srednji, 2 = Težek")]
    public int difficulty = 1;

    private Vector2 currentTargetPos;
    private Vector3 nasStartPos;
    private GameObject UDP;
    private GameUDPServer UDPScript;
    public float resetValue;

    public GameObject PausePanel;
    public GameObject MenuPanel;
    public GameObject IgraPanel;

    void Start()
    {
       
        difficulty = PlayerPrefs.GetInt("SelectedDifficulty", 1);
        ApplyDifficultySettings(difficulty);

        // UDP 
        UDP = GameObject.Find("UDP");
        if (UDP != null)
            UDPScript = UDP.GetComponent<GameUDPServer>();

        currentTargetPos = transform.localPosition;
        if (nasTransform != null)
            nasStartPos = nasTransform.localPosition;
    }

    void Update()
    {
        if (puckTransform == null) return;

        reactionTimer += Time.deltaTime;
        if (reactionTimer >= reactionDelay)
        {
            currentTargetPos = ComputeTargetPosition();
            reactionTimer = 0f;
        }

        resetValue = gameManager.resetPuckOnGoal;
        if (resetValue == 1)
            nasTransform.localPosition = nasStartPos;

        if (UDPScript != null)
        {
            UDPScript.sendFloat3(resetValue, transform.localPosition.x, -transform.localPosition.y);
            Debug.Log("Poslano: " + resetValue + ", " + transform.localPosition.x + ", " + -transform.localPosition.y);
        }
        
        if (PausePanel.activeInHierarchy)
        {
            UDPScript.sendFloat3(2f, 0f, 0f);
        }
        if (MenuPanel.activeInHierarchy)
        {
            UDPScript.sendFloat3(1f, 0f, 0f);
        }
        if (IgraPanel.activeInHierarchy)
        {
            UDPScript.sendFloat3(1f, 0f, 0f);
        }

        MoveAI(currentTargetPos);
    }

    private Vector2 ComputeTargetPosition()
    {
        Vector3 puckPos = puckTransform.localPosition;
        Vector2 target;

        if (puckPos.x < centerLineX)
        {
            float desiredX = puckPos.x - offensiveApproachDistance;
            float desiredY = puckPos.y + offensiveYOffset;
            target = new Vector2(desiredX, desiredY);

            if (puckPos.x < hitThresholdX)
                HitPuck();
        }
        else
        {
            float desiredX = defenseX;
            float desiredY = puckPos.y + defensiveYOffset;
            target = new Vector2(desiredX, desiredY);
        }

        return target;
    }

    private void MoveAI(Vector2 targetPosition)
    {
        Vector2 cur = new Vector2(transform.localPosition.x, transform.localPosition.y);
        Vector2 nxt = Vector2.MoveTowards(cur, targetPosition, movementSpeed * Time.deltaTime);
        transform.localPosition = new Vector3(nxt.x, nxt.y, transform.localPosition.z);
    }

    private void HitPuck()
    {
        // Tvoji učinki udarca: animacija, zvok, sila na puck ...
    }
    public void SetDifficulty(int lvl)
    {
        difficulty = lvl;
        ApplyDifficultySettings(lvl);
    }
    private void ApplyDifficultySettings(int lvl)
    {
        switch (lvl)
        {
            case 0: // Lahek
                movementSpeed = 0.05f;
                reactionDelay = 0.04f;
                offensiveApproachDistance = 0.01f;
                hitThresholdX = -0.3f;
                break;

            case 2: // Tezek
                movementSpeed = 0.1f;
                reactionDelay = 0.02f;
                offensiveApproachDistance = 0.005f;
                hitThresholdX = -0.5f;
                break;

            default: // Srednji
                movementSpeed = 0.07f;
                reactionDelay = 0.03f;
                offensiveApproachDistance = 0.005f;
                hitThresholdX = -0.4f;
                break;
        }
    }
}
