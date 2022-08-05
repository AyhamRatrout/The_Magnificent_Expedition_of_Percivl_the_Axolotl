using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    private void Start()
    {
        this.pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        this.pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        this.pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        StartCoroutine(MainMenuLoader());
    }

    public void QuitGame()
    {
        StartCoroutine(Quitter());
    }

    private IEnumerator MainMenuLoader()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene("StartScene");
    }

    private IEnumerator Quitter()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        Application.Quit();
    }
}
