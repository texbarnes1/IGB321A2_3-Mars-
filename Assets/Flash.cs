using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    // the flashing text object
    public Text textRender;

    //Controls for the flashing effect
    public float flashSpeed = 3f;
    private bool flashUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flashUp == false)
        {
            textRender.color = Color.Lerp(textRender.color, new Color(textRender.color.r, textRender.color.g, textRender.color.b, 0), Time.deltaTime * flashSpeed);
            if (textRender.color.a <= 0.02f)
            {
                flashUp = true;
            }
        }
        else
        {
            textRender.color = Color.Lerp(textRender.color, new Color(textRender.color.r, textRender.color.g, textRender.color.b, 1), Time.deltaTime * flashSpeed);
            if (textRender.color.a >= 0.89)
            {
                flashUp = false;
            }
        }
    }
}
