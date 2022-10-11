using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
    
        if (other.tag == "Player")
        {
            other.transform.GetComponent<PlayerAvatar>().keyCards += 1;
    
            Destroy(this.gameObject);
        }
    }
    

}
