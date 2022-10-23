using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Section
{
    Intro = 0,
    Section1 = 1,
    Section2 = 2,
    Section2AfterAllGenAreDone = 3,
    PreSection3 = 4,
    Section3Intro = 5,
    Section3 = 6
}
public class Level2_dynamicMusic : MonoBehaviour
{
    public AudioMixerSnapshot[] tracks = new AudioMixerSnapshot[7];
    public int selectionIndex = 0;
    public float transistionSpeed = 0.5f;
    public Section section;

    public PowerManager powerManager;
    private void Start()
    {
        selectionIndex = (int)section;
        tracks[selectionIndex].TransitionTo(transistionSpeed);
    }
    private void Update()
    {
        if(powerManager != null)
        {
            if(powerManager.PowerOn == true)
            {
                if((int)section < 3)
                {
                    section = (Section)3;
                }
            }
        }
        if(selectionIndex != (int)section)
        {
            selectionIndex = (int)section;
            tracks[selectionIndex].TransitionTo(transistionSpeed);
        }
    }
}
