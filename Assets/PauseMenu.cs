using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject levelSelect;
    public GameObject black;

    private bool paused = false;
    public bool isMainMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!isMainMenu)
        {
            Resume();

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isMainMenu == false)
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                levelSelect.SetActive(false);
                paused = true;
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        levelSelect.SetActive(false);
        paused = false;
    }

    public void Restart()
    {
        black.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OpenLevelSelect()
    {
        pauseMenu.SetActive(false);
        levelSelect.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        pauseMenu.SetActive(true);
        levelSelect.SetActive(false);
    }

    public void SelectLevel(string level)
    {
        CheckpointControl.Checkpoint = Vector3.zero;
        black.SetActive(true);
        SceneManager.LoadScene(level);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void StartGame(string level)
    {
        SceneManager.LoadScene(level);
    }
}
