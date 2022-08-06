using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeCutscenes : MonoBehaviour
{
    [SerializeField] private CanvasGroup _page1;
    [SerializeField] private CanvasGroup _page2;
    [SerializeField] private CanvasGroup _page3;
    [SerializeField] private CanvasGroup _page4;
    [SerializeField] private CanvasGroup _page5;
    [SerializeField] private float readingTime1 = 5.0f;
    [SerializeField] private float readingTime2 = 5.0f;
    [SerializeField] private float readingTime3 = 5.0f;
    [SerializeField] private float readingTime4 = 5.0f;
    [SerializeField] private float readingTime5 = 5.0f;
    private Coroutine lastFade = null;
    private Coroutine lastCycle = null;
    private bool pageShowing = false;
    private bool moreThanFour = false;

    void Start()
    {
        //Start the screen black
        _page1.alpha = 0.0f;
        _page2.alpha = 0.0f;
        _page3.alpha = 0.0f;
        _page4.alpha = 0.0f;
        if(_page5 != null){
            moreThanFour = true;
            _page5.alpha = 0.0f;
        }

        StartCoroutine(PlayScene());

    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            Debug.Log(SceneManager.sceneCountInBuildSettings);
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            if((SceneManager.GetActiveScene().buildIndex + 1) != SceneManager.sceneCountInBuildSettings) {
                SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
            } else {
                SceneManager.LoadScene(0);
            }
        }
    }
    private IEnumerator PlayScene(){
        yield return new WaitForSeconds(1.0f);
        lastCycle = StartCoroutine(CycleThrough(_page1, readingTime1));

        while(pageShowing){yield return null;}
        lastCycle = StartCoroutine(CycleThrough(_page2, readingTime2));

        while(pageShowing){yield return null;}
        lastCycle = StartCoroutine(CycleThrough(_page3, readingTime3));

        while(pageShowing){yield return null;}
        lastCycle = StartCoroutine(CycleThrough(_page4, readingTime4));

        if(moreThanFour){
            while(pageShowing){yield return null;}
            lastCycle = StartCoroutine(CycleThrough(_page5, readingTime5));
        }

        // this part was commented
        while(pageShowing){yield return null;}
        // UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
        if((SceneManager.GetActiveScene().buildIndex + 1) != SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
        } else {
            SceneManager.LoadScene(0);
        }
        
    }
    private IEnumerator CycleThrough(CanvasGroup canvas, float readingTime){
        pageShowing = true;

        lastFade = StartCoroutine(FadeIn(canvas));
        yield return new WaitForSeconds(2.0f);
        StopCoroutine(lastFade);
        Debug.Log("Fade In Stopped");

        //delay for reading
        yield return new WaitForSeconds(readingTime);

        lastFade = StartCoroutine(FadeOut(canvas));
        yield return new WaitForSeconds(2.0f);
        StopCoroutine(lastFade);

        pageShowing = false;
        StopCoroutine(lastCycle);
    }

    private IEnumerator FadeIn(CanvasGroup canvas){
        while(canvas.alpha <= 1.0f){
            canvas.alpha += Time.deltaTime;
            Debug.Log("Fade In Running");
            yield return null;
        }
    }

    private IEnumerator FadeOut(CanvasGroup canvas){
        while(canvas.alpha >= 0.0f){
            canvas.alpha -= Time.deltaTime;
            Debug.Log("Fade Out Running");
            yield return null;
        }
    }
}
