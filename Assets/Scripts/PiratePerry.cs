using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiratePerry : MonoBehaviour
{
    [SerializeField] private GameObject feather;
    [SerializeField] private float featherSpeed = 6.0f;
    [SerializeField] private float perrySpeed = 6.0f;
    [SerializeField] private float flapTime = 1.0f; //how long for flap animation
    [SerializeField] private float branchMovementTimer = 15.0f; // how long before he moves to new branch
    [SerializeField] private float attackTimer = 15.0f;
    [SerializeField] private Slider healthBar;

    //some magic number times baby 
    private Vector3 middleBranch = new Vector3(3.4f, -0.7f, 0.0f);
    private Vector3 leftBranch = new Vector3(-4.1f, 3.8f, 0.0f);
    private Vector3 rightBranch = new Vector3(10.4f, 3.8f, 0.0f);
    private Vector3[] branchPositions;
    private Coroutine fight;
    private Coroutine lastAttack;
    private Animator animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        healthBar.maxValue = GetComponent<EnemyBehavior>().health;
        branchPositions = new Vector3[] {leftBranch, middleBranch, rightBranch};
        transform.Translate(0.0f,11.0f,0.0f);
        StartCoroutine(FlyToBranch(middleBranch));
        fight = StartCoroutine(StartFight());
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = GetComponent<EnemyBehavior>().health;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if(other.CompareTag("Player")){
            other.GetComponent<PlayerHealth>().TakeDamage(1);
        }        
    }

    IEnumerator StartFight(){
        //wait for perry to get to branch
        yield return new WaitForSeconds(branchMovementTimer/2.0f);

        //start attack cycle
        while(true){
            lastAttack = StartCoroutine(TargetedFeather(4));
            yield return new WaitForSeconds(attackTimer);
            lastAttack = StartCoroutine(PatternCircle(3));
            yield return new WaitForSeconds(attackTimer);
            lastAttack = StartCoroutine(PatternSpiral());
            yield return new WaitForSeconds(attackTimer);
            lastAttack = StartCoroutine(TargetedFeather(4));
            yield return new WaitForSeconds(attackTimer);
            lastAttack = StartCoroutine(PatternCircle(1));
            yield return new WaitForSeconds(attackTimer);
            lastAttack = StartCoroutine(FlyToBranch(branchPositions[Random.Range(0,3)]));
            yield return new WaitForSeconds(branchMovementTimer);
        }
    }

    private void shootFeather(string direction){
        //instantiate feather
        var newFeather = Instantiate(feather, this.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        var velocity = new Vector2(0.0f,0.0f);

        //set direction and rotation
        if(direction.Contains("down")) {
            velocity += Vector2.down;
            newFeather.transform.Rotate(new Vector3(0,0,45));
        }
        if(direction.Contains("up")) {
            velocity += Vector2.up;
            newFeather.transform.Rotate(new Vector3(0,0,-135));
        }
        if(direction.Contains("left")) {
            velocity += Vector2.left;
            newFeather.transform.Rotate(new Vector3(0,0,-45));
        }
        if(direction.Contains("right")) {
            velocity += Vector2.right;
            newFeather.transform.Rotate(new Vector3(0,0,135));
        }

        //
        if((direction.Contains("up") && direction.Contains("right"))){
            newFeather.transform.Rotate(new Vector3(180,180,0));
        }
        if((direction.Contains("up") && direction.Contains("left"))){
            newFeather.transform.Rotate(new Vector3(0,180,0));
        }
        if((direction.Contains("down") && direction.Contains("right"))){
            newFeather.transform.Rotate(new Vector3(180,0,0));
        }

        //add speed
        newFeather.velocity = velocity.normalized * featherSpeed;

    }

    private void shootFeather(Vector3 target){
        //make feather
        var newFeather = Instantiate(feather, this.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        //find vector towards the target, will normalize later to adjust speed properly
        var velocity = target - this.transform.position;
        //find angle to target
        var angle = Vector3.Angle(velocity.normalized, Vector3.right.normalized);
        if(target.y < transform.position.y) {angle = angle * -1;}
        newFeather.transform.Rotate(0.0f, 0.0f, angle + 135); //had to offset for sprite rotation
        newFeather.velocity = velocity.normalized * featherSpeed;
    }

    IEnumerator PatternCircle(int numberOfFeathers){
        for(int i = 0; i < numberOfFeathers; i++){
            shootFeather("upleft");
            shootFeather("up");
            shootFeather("upright");
            shootFeather("right");
            shootFeather("downright");
            shootFeather("down");
            shootFeather("downleft");
            shootFeather("left");
            yield return new WaitForSeconds(0.5f);
        }
        yield break;      
    }

    IEnumerator PatternSpiral(){
        yield return new WaitForSeconds(0.3f);
        shootFeather("upleft");
        yield return new WaitForSeconds(0.3f);
        shootFeather("up");
        yield return new WaitForSeconds(0.3f);
        shootFeather("upright");
        yield return new WaitForSeconds(0.3f);
        shootFeather("right");
        yield return new WaitForSeconds(0.3f);
        shootFeather("downright");
        yield return new WaitForSeconds(0.3f);
        shootFeather("down");
        yield return new WaitForSeconds(0.3f);
        shootFeather("downleft");
        yield return new WaitForSeconds(0.3f);
        shootFeather("left");
    }

    IEnumerator TargetedFeather(int numberOfFeathers){
        var player = GameObject.FindGameObjectWithTag("Player");
        for(int i = 0; i < numberOfFeathers; i++){
            shootFeather(player.transform.position);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator FlyToBranch(Vector3 branchPosition){

        //disable collider for sitting shape
        var polygonCollider = this.GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;

        //animate flap
        animator.SetBool("flying", true);
        yield return new WaitForSeconds(flapTime);

        //enable collider for flight shape
        var boxCollider = this.GetComponent<BoxCollider2D>();
        boxCollider.enabled = true;

        //fly up screen
        while(this.transform.position.y < 11.0f){
            this.transform.Translate(0.0f, perrySpeed * Time.deltaTime, 0.0f);
            yield return null;
        }
        //move under new branch position off screen
        this.transform.position = new Vector3(branchPosition.x, -11.0f, 0.0f);
        //fly up to branch
        while(this.transform.position.y < branchPosition.y){
            this.transform.Translate(0.0f, perrySpeed * Time.deltaTime, 0.0f);
            yield return null;
        }
        this.transform.position = branchPosition;

        //disable collider for flight shape
        boxCollider.enabled = false;

        //animate flap
        animator.SetBool("flying", false);
        yield return new WaitForSeconds(flapTime);

        //enable collider for sitting shape
        polygonCollider.enabled = true;
        
    }

    public IEnumerator Die(){
        //disable colliders and stop attacks so he doesn't hurt player
        this.GetComponent<PolygonCollider2D>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        StopCoroutine(fight);
        StopCoroutine(lastAttack);

        var feetPosition = transform.position - new Vector3 (0.0f, 0.5f, 0.0f);
        //rotate around branch by feet
        while(transform.rotation.eulerAngles.x < 180.0f){
            transform.RotateAround(feetPosition, Vector3.right, (5.0f * Time.deltaTime));
            yield return null;
        }

        //set sprite layer so he falls behind ground like drawing
        GetComponent<SpriteRenderer>().sortingLayerName = "Default";

        //fall to ground (stick halfway out)
        while(transform.position.y > -2.8){
            this.transform.Translate(0.0f,  0.01f * Time.deltaTime, 0.0f);
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        Debug.Log("Destroyed by perry script");
        Destroy(this.gameObject);
    }
}
