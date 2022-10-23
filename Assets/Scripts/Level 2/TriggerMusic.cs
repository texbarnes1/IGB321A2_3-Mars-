using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusic : MonoBehaviour
{
    // Start is called before the first frame update
    public Level2_dynamicMusic master;
    public int trackNum;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(master.selectionIndex < trackNum)
            {
                master.section = (Section)trackNum;
            }
        }    
    }
}
