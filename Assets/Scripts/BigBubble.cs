using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigBubble : MonoBehaviour
{
    [SerializeField] private GameObject bubble;
    [SerializeField] private float bubbleSpeed = 6.0f;
    [SerializeField] private float bigBubbleSpeed = 6.0f;
    [SerializeField] private float attackTimer = 1.0f;
    [SerializeField] private Slider healthBar;
    private Coroutine fight;
    private float roomBoundaryLeft = -4.5f;
    private float roomBoundaryRight = 9.5f;
    private float nextAngle = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = GetComponent<EnemyBehavior>().health;
        fight = StartCoroutine(StartFight());
        Flip();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = GetComponent<EnemyBehavior>().health;
        
        //move around
        if(transform.position.x < roomBoundaryLeft){
            bigBubbleSpeed = bigBubbleSpeed * -1.0f;
            Flip();
            transform.position = new Vector3(roomBoundaryLeft, transform.position.y, transform.position.z);
        }
        if(transform.position.x > roomBoundaryRight) {
            bigBubbleSpeed = bigBubbleSpeed * -1.0f;
            Flip();
            transform.position = new Vector3(roomBoundaryRight, transform.position.y, transform.position.z);
        }


        transform.Translate(bigBubbleSpeed * Time.deltaTime, Mathf.Sin(nextAngle) * Time.deltaTime, 0.0f);
        Debug.Log(Mathf.Sin(nextAngle));
        nextAngle += Time.deltaTime;


    }

    IEnumerator StartFight(){
        yield return new WaitForSeconds(attackTimer);

        //start attack cycle
        while(true){
            StartCoroutine(TargetedBubble(4));
            yield return new WaitForSeconds(attackTimer);
        }
    }
    private void shootBubble(Vector3 target){
        //make bubble
        var newFeather = Instantiate(bubble, this.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        //find vector towards the target, will normalize later to adjust speed properly
        var velocity = target - this.transform.position;
        newFeather.velocity = velocity.normalized * bubbleSpeed;
    }

    IEnumerator TargetedBubble(int numberOffBubbles){
        var player = GameObject.FindGameObjectWithTag("Player");
        for(int i = 0; i < numberOffBubbles; i++){
            shootBubble(player.transform.position);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator Die(){
        //disable colliders and stop attacks so he doesn't hurt player
        this.GetComponent<CircleCollider2D>().enabled = false;
        StopCoroutine(fight);

        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }

    private void Flip()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
