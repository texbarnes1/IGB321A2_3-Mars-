using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public bool locked = false;

    public float openDuration = 0; //set to zero to stay open permenantly
    public bool isOpen = false;

    private float timeOpen = 0;
    public Animator doorAnimation;
    // Start is called before the first frame update
    void Start()
    {
        Close();
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
        doorAnimation.Play("Close");
        isOpen = false;
        timeOpen = 0;
    }


    public void Open() // opens the door
    {
        doorAnimation.Play("Open");
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
            }

        }
    }
    void OnTrigger(Collider other)
    {
        if (other.tag == "Player")
        {
            if (locked == false)
            {
                if (isOpen == false)
                {
                    Open();
                }
                timeOpen = 0;
            }
        }
    }
}
