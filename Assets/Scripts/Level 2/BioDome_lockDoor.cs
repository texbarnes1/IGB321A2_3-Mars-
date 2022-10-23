using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioDome_lockDoor : MonoBehaviour
{
    public LockedDoor door;
    public PowerManager powerManager;
    public Text ToLower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(powerManager.PowerOn == true)
        {
            door.locked = false;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            door.locked = true;
            ToLower.text = "POWER IS REQUIRED";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ToLower.text = "";
        }
    }
}
