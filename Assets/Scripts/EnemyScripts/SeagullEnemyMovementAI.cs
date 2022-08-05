using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof(Seeker))]
public class SeagullEnemyMovementAI : MonoBehaviour
{
    public Transform target;
    public float updateRate = 2f;
    private Seeker seeker;
    private Rigidbody2D rigidBody;

    public float attackRange = 10.0f;

    private bool isBeingAttacked = false;

    public Transform enemyGFX;

    public Path path;
    public float speed = 500f;
    public ForceMode2D forceMode;

    [HideInInspector]
    public bool isPathEnded = false;

    public float nextWaypointDistance = 3f;

    private int currentWwayPoint = 0;

    private bool isSearchingForPlayer = false;

    private void Start()
    {
        this.seeker = this.GetComponent<Seeker>();
        this.rigidBody = this.GetComponent<Rigidbody2D>();

        if(this.target == null)
        {
            if (!isSearchingForPlayer)
            {
                isSearchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
    }

    public void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            this.currentWwayPoint = 0;
        }
    }

    IEnumerator SearchForPlayer()
    {
        var searchResult = GameObject.FindGameObjectWithTag("Player");
        if(searchResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            isSearchingForPlayer = false;
            target = searchResult.transform;
            StartCoroutine(UpdatePath());
            yield return false; 
        }
    }
    
    IEnumerator UpdatePath()
    {
        if (this.target == null)
        {
            if (!isSearchingForPlayer)
            {
                isSearchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return false;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void FixedUpdate()
    {
        if (Vector3.Distance(this.transform.position, this.target.position) <= this.attackRange)
        {
            if (!this.isBeingAttacked)
            {
                this.isBeingAttacked = true;
                seeker.StartPath(transform.position, target.position, OnPathComplete);
                StartCoroutine(UpdatePath());
            } 
        }

        if (this.target == null)
        {
            if (!isSearchingForPlayer)
            {
                isSearchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        if (this.path == null)
        {
            return;
        }
        if (this.currentWwayPoint >= path.vectorPath.Count)
        {
            if (isPathEnded)
            {
                return;
            }
            isPathEnded = true;
            return;
        }
        else
        {
            isPathEnded = false;
        }

        Vector3 dir = ((Vector2)path.vectorPath[currentWwayPoint] - rigidBody.position).normalized;
        Vector2 force = dir * speed * Time.fixedDeltaTime;

        this.rigidBody.AddForce(force);
        float distance = Vector2.Distance(this.rigidBody.position, path.vectorPath[currentWwayPoint]);
        

        if (distance < nextWaypointDistance)
        {
            currentWwayPoint++;
            return;
        }

        Debug.Log(this.transform.rotation.eulerAngles);
        if (this.rigidBody.velocity.x >= 0.01)
        {
            //this.transform.rotation = Quaternion.LookRotation(new Vector3(this.transform.localScale.x, -this.transform.localScale.y, this.transform.localScale.z));
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            //this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
        if (this.rigidBody.velocity.x <= -0.01)
        {
            //this.transform.rotation = Quaternion.LookRotation(new Vector3(this.transform.localScale.x, -this.transform.localScale.y, this.transform.localScale.z));
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            //this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }

        /*if (this.target.position.x > this.transform.position.x)
        {
            Debug.Log("facingRight");
            this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
        else
        {
            Debug.Log("facingLeft");
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }*/

    }
}
