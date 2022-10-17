using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailCar : MonoBehaviour
{
    public Animation anim;
    public LockedDoor door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && door.doorAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.name == "IdleClosed")
        {
            other.gameObject.SetActive(false);
            anim.Play();
        }
    }
}
