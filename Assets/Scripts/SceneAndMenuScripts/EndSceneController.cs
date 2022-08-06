using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneController : MonoBehaviour
{
    public void PlayAgain()
    {
        Debug.Log("play");
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
