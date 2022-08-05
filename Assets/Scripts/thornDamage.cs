using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thornDamage : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject respawn;
    [SerializeField] private CanvasGroup canvas;
    // Start is called before the first frame update
    void Start()
    {
        if(respawn == null){
            respawn = GameObject.FindWithTag("Respawn");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D (Collider2D other) {
        if(other.CompareTag("Player") && !other.isTrigger) {
            StartCoroutine(FadeOut(canvas));
        }
    }

    private IEnumerator FadeOut(CanvasGroup canvas){
        while(canvas.alpha < 1.0f){
            canvas.alpha += Time.deltaTime;
            yield return null;
        }
        player.transform.position = respawn.transform.position;
        if (player.GetComponent<PlayerHealth>().invincible == false) {
            player.GetComponent<PlayerHealth>().TakeDamage(1);
        }
        yield return new WaitForSeconds(1.0f);
        canvas.alpha = 0.0f;
    }
}
