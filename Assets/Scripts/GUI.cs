using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GUI : MonoBehaviour {

    GameObject player;

    public Slider healthbar;
    public Text ammoText;
    public Text fuelText;
    public Text keyText;
    public Text oxygenText;
    public GameObject levelCompleteText;

    //Level TransitionElements
    public string nextLevel = "Level 1";
    public Image screenFade;
    public float fadeSpeed = 2;
    public CanvasGroup screenOverlays;

    //Oxygen Objects
    public Slider oxygenbar;
    public GameObject oxygenAlert;
    public CanvasGroup oxygenPopup;
    public AudioLowPassFilter lowPassFilter;
    public float lowPassFrequency = 610;
    public float lowPassResonance = 1.4f;

    //Post processing effects
    public PostProcessVolume oxygenAtmosphere;
    public float atmosphereShiftSpeed = 2;
    private LensDistortion lensDistort;
    private ChromaticAberration chromaticAbberation;
    // Use this for initialization
    void Start () {
        lensDistort = oxygenAtmosphere.profile.GetSetting<LensDistortion>();
        chromaticAbberation = oxygenAtmosphere.profile.GetSetting<ChromaticAberration>();

        if (Camera.main.GetComponent<AudioLowPassFilter>() == null)
        {
            lowPassFilter = Camera.main.gameObject.AddComponent<AudioLowPassFilter>();
        }
        else
        {
            lowPassFilter = Camera.main.GetComponent<AudioLowPassFilter>();
        }
        lowPassFilter.cutoffFrequency = lowPassFrequency;
        lowPassFilter.lowpassResonanceQ = lowPassResonance;

    }
	
	// Update is called once per frame
	void Update () {

        if (!player) {
            player = GameObject.FindGameObjectWithTag("Player");
            healthbar.value = 0;
        }
        else if (player) {
            healthbar.value = player.GetComponent<PlayerAvatar>().health;
            ammoText.text = "Ammo: " + player.GetComponent<PlayerAvatar>().ammo.ToString();
            fuelText.text = "Fuel: " + player.GetComponent<PlayerAvatar>().fuel.ToString();
            keyText.text = "Key Cards: " + player.GetComponent<PlayerAvatar>().keyCards.ToString();
            OxygenEffects();
        }


        //Level Transition
        if (GameManager.instance.levelComplete)
        {
            screenFade.color = Color.Lerp(screenFade.color, Color.black, fadeSpeed * Time.unscaledDeltaTime);
            screenOverlays.alpha = Mathf.Lerp(screenOverlays.alpha, 0, fadeSpeed * 2 * Time.unscaledDeltaTime);

            if (levelCompleteText.activeInHierarchy == false) // Load the next Level
            {
                StartCoroutine(GameManager.instance.LoadLevel(nextLevel));
                levelCompleteText.SetActive(true);
            }
            
        }

        //Level Transition
        if (GameManager.instance.playerDead)
        {
            screenFade.color = Color.Lerp(screenFade.color, Color.black, fadeSpeed * Time.unscaledDeltaTime);
            screenOverlays.alpha = Mathf.Lerp(screenOverlays.alpha, 0, fadeSpeed * 2 * Time.unscaledDeltaTime);

            if (levelCompleteText.activeInHierarchy == false) // Load the next Level
            {
                StartCoroutine(GameManager.instance.LoadLevel(GameManager.instance.thisLevel));
                levelCompleteText.SetActive(true);
            }

        }


    }

    public void OxygenEffects()
    {
        if (player.GetComponent<PlayerOxygen>().isLosingOxygen)
        {
            lowPassFilter.enabled = true;
            oxygenAlert.SetActive(true);

            if (player.GetComponent<PlayerOxygen>().OxygenRemaining >= 95)
            {
                oxygenPopup.alpha = 1;
            }

            //Edit Post Processing to give a low oxygen effect
            lensDistort.intensity.value = Mathf.Lerp(lensDistort.intensity.value, -(100 - player.GetComponent<PlayerOxygen>().OxygenRemaining) / 2, atmosphereShiftSpeed * Time.deltaTime);
            chromaticAbberation.intensity.value = Mathf.Lerp(chromaticAbberation.intensity.value, (100 - player.GetComponent<PlayerOxygen>().OxygenRemaining) / 100, atmosphereShiftSpeed* Time.deltaTime);
        }
        else
        {
            lowPassFilter.enabled = false;
            oxygenAlert.SetActive(false);

            //Edit Post Processing to give a low oxygen effect
            lensDistort.intensity.value = Mathf.Lerp(lensDistort.intensity.value, 0, atmosphereShiftSpeed * Time.deltaTime);
            chromaticAbberation.intensity.value = Mathf.Lerp(chromaticAbberation.intensity.value, 0, atmosphereShiftSpeed * Time.deltaTime);
        }

        
        
        
        // if the player runs out of oxygen the screen begins to fade to black
        if (player.GetComponent<PlayerOxygen>().OxygenRemaining <= 0)
        {
            screenFade.color = Color.Lerp(screenFade.color, Color.black, fadeSpeed/4 * Time.deltaTime);
        }

        oxygenPopup.alpha -= 0.5f * Time.deltaTime;
        oxygenbar.value = player.GetComponent<PlayerOxygen>().OxygenRemaining;
        //oxygenText.text = "Oxygen: " + player.GetComponent<PlayerOxygen>().OxygenRemaining.ToString("F0") + "%";
    }
}
