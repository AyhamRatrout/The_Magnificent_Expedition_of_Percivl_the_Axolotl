using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnemyMovementAI : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 1.0f;

    private bool isMovingLeft = true;

    public Transform groundCheck;

    private CapsuleCollider2D fishCollider;

    private GameObject player;

    private float distance = 0.0f;

    private float minimumFollowRange = 5.0f;

    private void Start()
    {
        this.fishCollider = this.GetComponent<CapsuleCollider2D>();
        this.player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        this.transform.Translate(Vector2.left * this.movementSpeed * Time.deltaTime);
        
        if (!this.groundCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Platforms")))
        {
            if (this.isMovingLeft)
            {
                this.transform.eulerAngles = new Vector3(0, -180, 0);
                isMovingLeft = false;
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                isMovingLeft = true;
            }
        }
        IsPlayerInRange();
    }

    private void IsPlayerInRange()
    {
        this.distance = Vector2.Distance(this.transform.position, this.player.transform.position); ;
        if(this.distance <= this.minimumFollowRange && 
            Mathf.Abs(this.transform.position.y - this.player.transform.position.y) < 0.3f)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        if (this.transform.position.x < this.player.transform.position.x)
        {
            if (this.isMovingLeft)
            {
                this.transform.eulerAngles = new Vector3(0, -180, 0);
                this.transform.position = Vector2.MoveTowards(this.transform.position, this.player.transform.position, movementSpeed * Time.deltaTime);
                isMovingLeft = false;
            }
            else
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, this.player.transform.position, movementSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (this.isMovingLeft)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, this.player.transform.position, movementSpeed * Time.deltaTime);
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                this.transform.position = Vector2.MoveTowards(this.transform.position, this.player.transform.position, movementSpeed * Time.deltaTime);
                isMovingLeft = true;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.fishCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")) ||
            this.fishCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) ||
            collision.gameObject.tag == "SceneTransition")
        {
            if (this.isMovingLeft)
            {
                this.transform.eulerAngles = new Vector3(0, -180, 0);
                isMovingLeft = false;
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                isMovingLeft = true;
            }
        }
    }

}
