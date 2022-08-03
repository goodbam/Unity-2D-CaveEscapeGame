using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformIgnore : MonoBehaviour
{
    [SerializeField] public CompositeCollider2D platformTilemapCollider;

    bool Isboos;

    // OnTriggerStay2D : 접촉 중인 콜라이더를 계속해서 반환한다.
    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Isboos = true;
            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), platformTilemapCollider, Isboos);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Isboos = false;
            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), platformTilemapCollider, false);
        }
    }
}
