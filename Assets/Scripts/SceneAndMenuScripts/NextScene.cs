using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private bool isBossRoom = false;
    [SerializeField] private GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        if(isBossRoom && (boss == null)){
            boss = GameObject.FindWithTag("Enemy");
        }
        canvas.alpha = 1.0f;
        StartCoroutine(FadeIn(canvas));
    }

    // Update is called once per frame
    void Update()
    {
        if(isBossRoom && (boss == null)){
            StartCoroutine(FadeOut(canvas));
        }
    }
    
    private void OnTriggerEnter2D (Collider2D other) {

        if(other.CompareTag("Player") && !other.isTrigger) {
            StartCoroutine(FadeOut(canvas));
            //not sure why it doesn't recognize this lamo
        }
    }

    private IEnumerator FadeOut(CanvasGroup canvas){
        while(canvas.alpha < 1.0f){
            canvas.alpha += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Loading next scene");
        if((SceneManager.GetActiveScene().buildIndex + 1) != SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
        } else {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator FadeIn(CanvasGroup canvas){
        while(canvas.alpha > 0.0f){
            canvas.alpha -= Time.deltaTime;
            yield return null;
        }
    }
}
