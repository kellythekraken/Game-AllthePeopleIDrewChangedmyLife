using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SideNPC : MonoBehaviour
{
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    [YarnCommand("pos")]
    void ChangePose(string anim, float transitionTime = 0f)
    {
        _animator.CrossFadeInFixedTime(anim,transitionTime,0);
    }
}
