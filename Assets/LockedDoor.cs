using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public bool locked = false;

    public float openDuration = 0; //set to zero to stay open permenantly
    public bool isOpen = false;

    //for airlocks
    public LockedDoor airlockPartner = null;

    private float timeOpen = 0;
    public Animator doorAnimation;

    public GameObject keyAlert; // the alert that the player has no key card
    // Start is called before the first frame update
    void Start()
    {
        Close();
        if (locked)
        {
            openDuration = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen && openDuration > 0)
        {
            if (timeOpen < openDuration)
            {
                timeOpen += Time.deltaTime;
            }
            else
            {
                Close();
            }
        }
    }

    public void Close() // closes the door
    {
        doorAnimation.SetFloat("DoorState", 0);
        isOpen = false;
        timeOpen = 0;
    }


    public void Open() // opens the door
    {
        //print("beans");
        doorAnimation.SetFloat("DoorState", 1);
        isOpen = true;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            if (locked)
            {
                if (other.transform.GetComponent<PlayerAvatar>().keyCards >= 1 && isOpen == false)
                {
                    other.transform.GetComponent<PlayerAvatar>().keyCards -= 1;
                    Open();
                }
                else if (keyAlert != null)
                {
                    keyAlert.SetActive(true);
                }
            }

        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (locked == false)
            {
                if (isOpen == false && (airlockPartner == null || airlockPartner.doorAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.name == "IdleClosed"))
                {
                    Open();
                }
                timeOpen = 0;
            }
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (keyAlert != null && keyAlert.activeInHierarchy == true)
            {
                keyAlert.SetActive(false);
            }
        }
    }
}
