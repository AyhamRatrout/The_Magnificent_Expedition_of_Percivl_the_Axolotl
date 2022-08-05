using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int health = 5;
    public bool invincible = false;
    [SerializeField] public float invincibilityDurationSeconds = 3.0f;
    [SerializeField] public int contactDamage = 1;
    [SerializeField] public float horizontalKnockback = 5.0f;
    [SerializeField] public float verticalKnockback = 4.0f;
    [SerializeField] public float knockbackJumpThreshold = 8.0f;
    [SerializeField] private Image[] healthBarHearts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //die
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }  
        Debug.Log(health);
        HealthBar();
        
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        // Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.CompareTag("Enemy")) {
            if (invincible == false) {
                TakeDamage(contactDamage);
                // knockback
                float newVerticalKnockback;
                float forceDirection = Mathf.Sign(transform.position.x - collision.gameObject.transform.position.x);
                Vector3 playerVelocity = GetComponent<Rigidbody2D>().velocity;
                if (playerVelocity.y > knockbackJumpThreshold) {
                    newVerticalKnockback = 0.0f;
                } else {
                    newVerticalKnockback = verticalKnockback;
                }
                
                Vector2 forceVector = new Vector2(horizontalKnockback * forceDirection, newVerticalKnockback);
                GetComponent<Rigidbody2D>().AddForce(forceVector, ForceMode2D.Impulse);

            }
            Debug.Log(health);
        }
    }


    public void TakeDamage(int damage)
    {
        if (invincible == false) {
            health -= damage;
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        Debug.Log("Player turned invincible!");
        invincible = true;
        var sprite = GetComponent<SpriteRenderer>();
        var flicker = StartCoroutine(Flicker(sprite));
        yield return new WaitForSeconds(invincibilityDurationSeconds);
        StopCoroutine(flicker);
        sprite.enabled = true;
        invincible = false;
        Debug.Log("Player is no longer invincible!");
    }

    private IEnumerator Flicker(SpriteRenderer sprite){
        while(true){
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void HealthBar() {
        for (int i = 0; i < healthBarHearts.Length; i++) {
            if(i < health) {
                healthBarHearts[i].color = Color.white;
            } else {
                healthBarHearts[i].color = Color.black;
            }
        }
    }

}
