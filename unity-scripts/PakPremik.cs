using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PakPremik : MonoBehaviour
{
    private GameObject pak;
    private GameObject UDP;
    private GameUDPServer UDPScript;


    void Start()
    {
        UDP = GameObject.Find("UDP");
        if (GameObject.Find("UDP"))
        {
            UDP = GameObject.Find("UDP");
            UDPScript = UDP.GetComponent<GameUDPServer>();
            print("UDP GameObject found");
        }
        pak = GameObject.Find("Pak");

    }
    void Update()
    {
        Vector3 pakPos = UDPScript.getPosition2();

        


        //  SKALIRANJE
        Vector3 unityPosPak = new Vector3(
            pakPos.x,
            pakPos.y * (-1),
            pakPos.z * 0
        );
        pak.transform.localPosition = unityPosPak;

       

    }
    private void OnCollisionEnter(Collision collision)
    {
        // preverimo, ali je objekt igralec ali nasprotnik
        if (collision.gameObject.CompareTag("Igralec") ||
            collision.gameObject.CompareTag("Nasprotnik") ||
            collision.gameObject.CompareTag("Miza"))
        {
            // predvajaj trk zvok
            if (AudioManager.instance != null)
                AudioManager.instance.PlaySFX(AudioManager.instance.collisionSFX);
        }
    }


}