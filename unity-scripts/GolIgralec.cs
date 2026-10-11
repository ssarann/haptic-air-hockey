using UnityEngine;

public class GolIgralec : MonoBehaviour
{
    public bool isPlayerGoal = true; // true = igralec, false = nasprotnik

    void OnTriggerEnter(Collider other)
    {
        // Preveri, če objekt, ki vstopi v trigger, ima tag "Puck"
        if (other.gameObject.CompareTag("Pak"))
        {
            // Poišči GameManager (predpostavljamo, da ima objekt GameManager oznako "GameManager")
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            if (gm != null)
            {
                gm.GoalScored(isPlayerGoal);
            }
        }
    }
}
