using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Singleton Setup
    public static GameManager instance = null;

    public bool playerDead = false;

    public bool levelComplete = false;

    public string thisLevel;
    public string nextLevel;

    //public GameObject checkPrefab;
    //private GameObject checkpointControl;
    //
    //public Vector3 checkPosition = Vector3.zero;


    // Awake Checks - Singleton setup
    void Awake() {

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
        {     
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        
        //checkpointControl = GameObject.Find("CheckpointControl");
        //
        //if (checkpointControl == null)
        //{
        //    checkpointControl = (GameObject)Instantiate(checkPrefab);
        //    checkpointControl.name = "CheckpointControl";
        //}
    }

    // Use this for initialization
    void Start () {
        thisLevel = SceneManager.GetActiveScene().name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator LoadLevel(string level) {

        yield return new WaitForSecondsRealtime(3);

        SceneManager.LoadScene(level);
        //print(level);
    }
    public IEnumerator LoadNextLevel(string level)
    {

        yield return new WaitForSecondsRealtime(5);

        SceneManager.LoadScene(level);
        //print(level);
    }


}
