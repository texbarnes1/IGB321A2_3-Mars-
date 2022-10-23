using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSoundsmanager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> warningSounds;
    public PowerManager powerManager;
    void Start()
    {
        foreach(GameObject w in warningSounds)
        {
            w.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(powerManager.PowerOn)
        {
            foreach (GameObject w in warningSounds)
            {
                Destroy(w);
            }
            Destroy(this.gameObject);
        }
    }
}
