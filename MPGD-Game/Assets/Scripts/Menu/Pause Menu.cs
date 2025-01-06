using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject startGameUI;
    public GameObject Crosshair;
    public GameObject dialogue;

    public static bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
    }
    void Update()
    {
        if (!dialogue.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (settingsMenuUI.activeSelf)
                {
                    CloseSettings();
                }
                else if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Crosshair.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
        Crosshair.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void BacktoMenu()
    {
        pauseMenuUI.SetActive(false);
        startGameUI.SetActive(true);
    }
    public void StartGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
}
