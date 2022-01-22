using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    private Animator thisAnimator;
    void Awake()
    {
        thisAnimator =  GetComponent<Animator>();
    }
    public void Switch(){
        thisAnimator.SetBool("isOn",!thisAnimator.GetBool("isOn"));
    }
}
