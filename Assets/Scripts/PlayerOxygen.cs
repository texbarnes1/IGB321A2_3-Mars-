using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOxygen : MonoBehaviour
{
    public float oxygenRemaining = 100;

    public float OxygenRemaining { get { return oxygenRemaining; } }
    [SerializeField] private float oxygenMax = 100f;
    public bool isLosingOxygen = false;
    [SerializeField] private float oxygenLossRate = 5f;

    public float oxygenRefreshRate = 10f;
    public float damageOverTime = 2;
    public PlayerAvatar player;
    private void Start()
    {
        oxygenRemaining = oxygenMax;
        player = gameObject.GetComponent<PlayerAvatar>();
    }

    private void Update()
    {
        if (isLosingOxygen)
        {
            
            //Debug.Log(oxygenRemaining, this);
            if (oxygenRemaining <= 0)
            {
                oxygenRemaining = 0;
                player.takeDamage(damageOverTime * Time.deltaTime);
                //Debug.Log("Ran out of oxygen", this);
            }
            else
            {
                oxygenRemaining -= oxygenLossRate * Time.deltaTime;
            }
        }
        else 
        {
            if (oxygenRemaining < oxygenMax)
            {
                oxygenRemaining += oxygenRefreshRate * Time.deltaTime;
            }
            else
            {
                oxygenRemaining = oxygenMax;
            }
                
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
