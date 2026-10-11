using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgralecPremik : MonoBehaviour
{
    private GameObject igralec;
    private GameObject UDP;
    private GameUDPServer UDPScript;


    // Start is called before the first frame update
    void Start()
    {
        UDP = GameObject.Find("UDP");
        if (GameObject.Find("UDP"))
        {
            UDP = GameObject.Find("UDP");
            UDPScript = UDP.GetComponent<GameUDPServer>();
            //print("UDP GameObject found");
        }
        igralec = GameObject.Find("Igralec");
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 phantomPos = UDPScript.getPosition1();

        //igralec.transform.localPosition = UDPScript.getPosition1();


        //  SKALIRANJE
        Vector3 unityPos = new Vector3(
            phantomPos.x,  
            phantomPos.y*(-1),
            phantomPos.z*0
        );
        igralec.transform.localPosition = unityPos;
        //Debug.Log("Prejeto: " + phantomPos);
        //Debug.Log("Pretvorjeno v Unity: " + unityPos);



        //float data1 = Time.time;
        //float data2 = Mathf.Sin(Time.time);
        //float data3 = Mathf.Cos(Time.time);

        //Ni treba pošiljati podatkov nazaj
        //Vector3 playerPos = igralec.transform.localPosition;  // Trenutna pozicija igralca
        //float data1 = playerPos.x;
        //float data2 = playerPos.y;
        //float data3 = playerPos.z;
        //UDPScript.sendFloat3(data1, data2, data3);  // Pošlje Phantom robotu
        //Debug.Log("Poslano: x:  " + data1 + "   y:    " + data2 + "   z:    " + data3);


        //petri_dish.transform.localPosition = new Vector3(UDPScript.getData(9), UDPScript.getData(10), 0.378f);
        //UDPScript.sendPosBee(pos.x, pos.y, pos.z);

    }



}