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
        if(transitionTime!= 0f)
        {
            StartCoroutine(DelayedAnim(anim,transitionTime));
        }
        else{_animator.CrossFadeInFixedTime(anim,0,0);}
    }
    
    IEnumerator DelayedAnim(string anim, float transitionTime)
    {
        var randWait = Random.Range(.2f,.55f);
        yield return new WaitForSeconds(randWait);
        _animator.CrossFadeInFixedTime(anim,transitionTime,0);
    }
}
