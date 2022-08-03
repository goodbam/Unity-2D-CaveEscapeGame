using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    Rigidbody2D myRigidbody;
    Transform myTransform;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other) { 
        moveSpeed = -moveSpeed;
        FlipMonsterFacing();
    }

    void FlipMonsterFacing()
    {
        myTransform.localScale = new Vector3(-(Mathf.Sign(myRigidbody.velocity.x)),1f, 1f);
    }
}
