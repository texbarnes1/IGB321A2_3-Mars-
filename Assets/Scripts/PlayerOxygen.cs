using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOxygen : MonoBehaviour
{
    private float oxygenRemaining;
    public float OxygenRemaining { get { return oxygenRemaining; } }
    [SerializeField] private float oxygenMax = 100f;
    private bool isLosingOxygen = false;
    [SerializeField] private float oxygenLossRate = 5f;

    private void Start()
    {
        oxygenRemaining = oxygenMax;
    }

    private void Update()
    {
        if (isLosingOxygen)
        {
            oxygenRemaining -= oxygenLossRate * Time.deltaTime;
            Debug.Log(oxygenRemaining, this);
            if (oxygenRemaining <= 0)
            {
                Debug.Log("Ran out of oxygen", this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OxygenFree"))
        {
            isLosingOxygen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OxygenFree"))
        {
            isLosingOxygen = false;
        }
    }
}
