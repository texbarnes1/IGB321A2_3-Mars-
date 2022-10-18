using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOxygen : MonoBehaviour
{
    public float oxygenRemaining = 100;

    public float OxygenRemaining { get { return oxygenRemaining; } }
    [SerializeField] private float oxygenMax = 100f;
    private bool isLosingOxygen = false;
    [SerializeField] private float oxygenLossRate = 5f;

    public float oxygenRefreshRate = 10f;
    private void Start()
    {
        oxygenRemaining = oxygenMax;
    }

    private void Update()
    {
        if (isLosingOxygen)
        {
            
            //Debug.Log(oxygenRemaining, this);
            if (oxygenRemaining <= 0)
            {
                //Debug.Log("Ran out of oxygen", this);
            }
            else
            {
                oxygenRemaining -= oxygenLossRate * Time.deltaTime;
            }
        }
        else if(oxygenRemaining <= oxygenMax)
        {
            oxygenRemaining += oxygenRefreshRate * Time.deltaTime;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OxygenFree"))
        {
            isLosingOxygen = true;
        }
        else if (other.CompareTag("Airlock"))
        {
            isLosingOxygen = false;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("OxygenFree"))
    //    {
    //        isLosingOxygen = false;
    //    }
    //}
}
