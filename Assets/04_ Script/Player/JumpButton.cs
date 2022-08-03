using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public bool swich;

    public void ButtonDown(){
        swich = true;
    }

    public void ButtonUp(){
        swich = false;
    }

}
