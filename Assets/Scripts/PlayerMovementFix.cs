using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementFix : MonoBehaviour
{
    public PlayerMovementController _controller;
    float horizontalMove = 0;
    public float runSpeed = 40f;
    bool jump = false;
    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if(horizontalMove != 0) {
            anim.SetBool("walking", true);
        } else {
            anim.SetBool("walking", false);
        }

        if (Input.GetButtonDown("Jump") && GetComponent<Rigidbody2D>().velocity.y <= 4) {
            jump = true;
        }

    }
    void FixedUpdate() {
        _controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false; 
    }
}
