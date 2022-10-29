using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public Boss boss;
    public GameObject sliderControl;
    public LockedDoor door;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = sliderControl.GetComponent<Slider>();
        slider.maxValue = boss.health;

        sliderControl.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = boss.health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            sliderControl.SetActive(true);
            door.locked = true;
        }
        
    }
}
