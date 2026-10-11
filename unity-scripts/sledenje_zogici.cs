using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sledenje_zogici : MonoBehaviour
{
    [Header("Referenčni objekti")]
    public Transform arCamera;    
    public Transform opponent;    

    [Header("Parametri preslikave")]
    public Vector3 inputMultiplier = new Vector3(-1f, 0f, 1f);

    [Header("Glajenje (EMA)")]
    public int smoothingPeriod = 10;


    private Vector3Smoother smoother;
    private Vector3 cameraRefPoint;
    private Vector3 opponentStartPos;



    public GameManager gameManager;
    private GameObject UDP;
    private GameUDPServer UDPScript;
    public float resetValue;

    public GameObject PausePanel;
    public GameObject MenuPanel;
    public GameObject IgraPanel;

    void Start()
    {
        if (arCamera == null || opponent == null)
        {
            Debug.LogError("Nastavi arCamera in opponent v Inspectorju!");
            enabled = false;
            return;
        }

        cameraRefPoint = arCamera.position;
        opponentStartPos = opponent.position;

        smoother = new Vector3Smoother(smoothingPeriod);


        // UDP 
        UDP = GameObject.Find("UDP");
        if (UDP != null)
            UDPScript = UDP.GetComponent<GameUDPServer>();
    }

    void Update()
    {
        // ročni reset
        if (Input.GetKeyDown(KeyCode.R))
            cameraRefPoint = arCamera.position;

        Vector3 raw = arCamera.position - cameraRefPoint;

        Vector3 smooth = smoother.Update(raw);


        // Mapiranje
        float mappedX = smooth.z * inputMultiplier.x;
        float mappedZ = smooth.x * inputMultiplier.z;

        Vector3 target = new Vector3(
            opponentStartPos.x + mappedX,
            opponentStartPos.y,
            opponentStartPos.z + mappedZ
        );

        opponent.position = target;

        resetValue = gameManager.resetPuckOnGoal;

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
    }

}

public class Vector3Smoother
{
    private Vector3 avg;
    private readonly float alpha;

    public Vector3Smoother(int period)
    {
        alpha = 2f / (period + 1f);
        avg = Vector3.zero;
    }

    public Vector3 Update(Vector3 input)
    {
        avg = Vector3.Lerp(avg, input, alpha);
        return avg;
    }
}
