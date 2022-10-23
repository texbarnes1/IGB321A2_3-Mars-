using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [Header("Powerable Lights in level")]
    public List<Light> Lights;
    public List<Light> LightsRunWithoutPower;

    [Header("Info")]
    public bool PowerOn = false;

    [Header("Generators")]
    public Generator[] PowerBox;
    private int NumofGen = 0;
    public int GenOn = 0;

    [Header("PowerDoor")]
    public LockedDoor[] LockDoor;

    [Header("O2")]
    public GameObject[] o2;

    // Start is called before the first frame update
    void Start()
    {

        NumofGen = PowerBox.Length;

        GameObject[] g = GameObject.FindGameObjectsWithTag("PowerAbleLight");
        foreach (GameObject g2 in g)
        {
            Lights.Add(g2.GetComponent<Light>());
        }
        GameObject[] h = GameObject.FindGameObjectsWithTag("NonePowerLight");
        foreach (GameObject g2 in h)
        {
            LightsRunWithoutPower.Add(g2.GetComponent<Light>());
        }

        if (LockDoor != null)
        {
            foreach (LockedDoor l in LockDoor)
            {
                l.locked = true;
            }
        }

        //checking if lights that needs to be on or off
        if (PowerOn)
        {
            foreach(Light l in Lights)
            {
                l.enabled = true;
            }
            foreach(Light l in LightsRunWithoutPower)
            {
                l.enabled = false;
            }
            if (LockDoor != null)
            {
                foreach (LockedDoor l in LockDoor)
                {
                    l.locked = true;
                }
            }
        }
        else
        {
                foreach (Light l in Lights)
                {
                    l.enabled = false;
                }
                foreach (Light l in LightsRunWithoutPower)
                {
                    l.enabled = true;
                }
            
        }

    }

    public void TurnOnTheLights()
    {
        PowerOn = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOxygen>().isLosingOxygen = false;
        foreach (Light l in Lights)
        {
            l.enabled = true;
        }
        foreach (Light l in LightsRunWithoutPower)
        {
            l.enabled = false;
        }
        //destorying this after we finish turning on
        foreach(GameObject o in o2)
        {
            Destroy(o);
        }
        if (LockDoor != null)
        {
            foreach (LockedDoor l in LockDoor)
            {
                l.locked = false;
            }
        }
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
