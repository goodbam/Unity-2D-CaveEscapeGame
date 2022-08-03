using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlogMovement : MonoBehaviour
{
    private float TimeLeft = 2.0f;
    private float nextTime = 0.0f;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;


    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCollider;
    BoxCollider2D wallDetectionBox;
    Transform myTransform;

    RaycastHit2D rayHit;

    void Start(){
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CapsuleCollider2D>();
        myTransform = GetComponent<Transform>();
        wallDetectionBox = transform.GetChild(1).GetComponent<BoxCollider2D>();
        
    }

    void Update(){
        if(Time.time > nextTime){
            nextTime = Time.time + TimeLeft;
            Jump();
        }

        if(myRigidbody.velocity.y <= 0){
            //Debug.DrawRay(myRigidbody.position, new Vector2(-1.5f, 0f), new Color(0, 1, 0));
            rayHit = Physics2D.Raycast(myRigidbody.position, new Vector2(0f, -3f) , 1f, LayerMask.GetMask("Platfrom"));
            if(rayHit.distance < 0.5f){
                myAnimator.SetBool("IsJumping",false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void Jump(){
        if(!myCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))){return;}

        myRigidbody.velocity = new Vector2(moveSpeed, jumpSpeed);
        myAnimator.SetBool("IsJumping",true);
    }

    void FlipEnemyFacing()
    {
        myTransform.localScale = new Vector3(Mathf.Sign(myRigidbody.velocity.x),1f, 1f);
    }
}
