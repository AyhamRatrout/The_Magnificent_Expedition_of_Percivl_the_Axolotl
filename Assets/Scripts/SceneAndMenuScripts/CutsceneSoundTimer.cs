using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSoundTimer : MonoBehaviour
{
    [SerializeField] CanvasGroup canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(canvas.alpha > 0.0f){
            Debug.Log("playing music");
            GetComponent<AudioSource>().Play();
        }
    }
}
