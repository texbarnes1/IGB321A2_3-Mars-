using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [Header("Powerable Lights in level")]
    public GameObject[] Lights;
    public GameObject[] LightsRunWithoutPower;

    [Header("Info")]
    public bool PowerOn = false;

    [Header("Generators")]
    public Generator[] PowerBox;
    private int NumofGen = 0;
    public int GenOn = 0;

    [Header("O2")]
    public GameObject o2;

    // Start is called before the first frame update
    void Start()
    {
        NumofGen = PowerBox.Length + 1;
        //checking if lights that needs to be on or off
        if(PowerOn)
        {
            foreach(GameObject l in Lights)
            {
                l.SetActive(true);
            }
            foreach(GameObject l in LightsRunWithoutPower)
            {
                l.SetActive(false);
            }    
        }
        else
        {
                foreach (GameObject l in Lights)
                {
                    l.SetActive(false);
                }
                foreach (GameObject l in LightsRunWithoutPower)
                {
                    l.SetActive(true);
                }
            
        }

    }

    public void TurnOnTheLights()
    {

        foreach (GameObject l in Lights)
        {
            l.SetActive(true);
        }
        foreach (GameObject l in LightsRunWithoutPower)
        {
            l.SetActive(false);
        }
        //destorying this after we finish turning on
        Destroy(this.gameObject);
    }
    
    public void GenCounterUp()
    {
        GenOn++;
        if(GenOn == NumofGen)
        {
            TurnOnTheLights();
        }
    }
}
