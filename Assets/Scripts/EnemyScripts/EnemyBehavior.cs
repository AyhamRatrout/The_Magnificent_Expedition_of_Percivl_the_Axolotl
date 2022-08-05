using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] public bool invincibleFrames = false;
    [SerializeField] public float invincibilityDurationSeconds = 2.0f;
    [SerializeField] private PiratePerry perry;
    [SerializeField] private BigBubble bubble;
    
    private bool invincible = false;

    private void Start()
    {

    }
    
    public void DecrementHealth()
    {
        if(invincibleFrames){
            if(!invincible){
                health--;
                StartCoroutine(BecomeTemporarilyInvincible());
            }
        }
        else{
            health--;
        }
    }
    
    private void Update()
    {
        if (health < 1)
        {
            if(perry != null){
                StartCoroutine(perry.GetComponent<PiratePerry>().Die());
            }
            else if(bubble != null){
                StartCoroutine(bubble.GetComponent<BigBubble>().Die());
            }
            else{
                Debug.Log("Destroy by enemy behavior");
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        invincible = true;
        var sprite = GetComponent<SpriteRenderer>();
        var flicker = StartCoroutine(Flicker(sprite));
        yield return new WaitForSeconds(invincibilityDurationSeconds);
        StopCoroutine(flicker);
        sprite.enabled = true;
        invincible = false;
        yield break;
    }

    private IEnumerator Flicker(SpriteRenderer sprite){
        while(true){
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
