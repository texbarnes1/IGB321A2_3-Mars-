using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    [Header("Setup")]
    public Light light;
    public Animation anim;
    private int CurrentTime;
    private Canvas GeneratorCountDown;
    private Text ToLower;
    private Text TimerText;

    [Header("Device info")]
    public LockedDoor Door;
    public int timer = 15;
    public bool PoweringON = false;

    [Header("Lockdown")]
    public GameObject[] Enemys;
    public float timeToSpawn = 1;
    public Light DoorLight;
    public Transform[] SpawnPoints;

    [Header("Room")]
    public Light[] Roomlights;
    private PowerManager PowerManager;

    [Header("Audio")]
    public Level2_dynamicMusic level2Music;
    public int trackNum;
    public AudioClip[] tracks;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        ToLower = GameObject.Find("ToLower").GetComponent<Text>();
        TimerText = GameObject.Find("Timer").GetComponent<Text>();
        PowerManager = GameObject.Find("POWER MANAGER").GetComponent<PowerManager>();
        anim.Play();
        ToLower.text = "";
        TimerText.text = "";
        if (DoorLight != null)
        {
            DoorLight.enabled = false;
        }
        if(Roomlights != null)
        {
            foreach(Light l in Roomlights)
            {
                l.enabled = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !PoweringON)
        {
            ToLower.text = "Press 'E' To power on the Generator";
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && !PoweringON)
        {
            if(Input.GetKey(KeyCode.E))
            {
                PowerUpPreCheck();
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            ToLower.text = "";
        }    
    }

    public void PowerUpPreCheck()
    {
        ToLower.text = "";
        if (!PoweringON)
        {
            //if we want to lock the player in a room
            if (Door != null)
            {
                Door.locked = true;
            }
            if (DoorLight != null)
            {
                DoorLight.enabled = true;
            }
            light.color = Color.yellow;
            StartCoroutine(LockDown(timer));
            PoweringON = true;
        }
    }
    private IEnumerator LockDown(int t)
    {
        StartCoroutine(SpawnEnemy(t));
        CurrentTime = t;

        if(level2Music != null)
        {
            level2Music.section = (Section)trackNum;
            source.clip = tracks[0];
            source.Play();
        }

        while (CurrentTime >= 0)
        {
            
            yield return new WaitForSeconds(1);

            //GUI
            float minutes = Mathf.FloorToInt(CurrentTime / 60);
            float seconds = Mathf.FloorToInt(CurrentTime % 60);
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            CurrentTime--;
        }

        //lockdown end
        PowerManager.GenCounterUp();
        anim.Stop();
        light.intensity = 1.5f;
        light.color = Color.green;
        if(Door != null)
        {
            Door.locked = false;
        }
        if (DoorLight != null)
        {
            DoorLight.enabled = false;
        }
        if (Roomlights != null)
        {
            foreach (Light l in Roomlights)
            {
                l.enabled = true;
            }
        }
        TimerText.text = "";
        if (level2Music != null)
        {
            level2Music.section = (Section)1;
            source.Stop();
            source.volume = 1;
            source.clip = tracks[1];
            source.Play();

        }
    }
    private IEnumerator SpawnEnemy(int t)
    {
        while(CurrentTime >= 0)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    void SpawnEnemy()
    {
        int r = Random.Range(0, SpawnPoints.Length);
        int e = Random.Range(0, Enemys.Length);
        Instantiate(Enemys[e], SpawnPoints[r]);
    }
}
