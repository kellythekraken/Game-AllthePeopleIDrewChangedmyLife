using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DemoIdleRestart : MonoBehaviour
{
    //if there are no action in one minute, restart the game
    [SerializeField] int maximumIdleTime = 60;
    float LastIdleTime;
    GameManager gm;
    SceneManager sceneManager;
    float idleTime = 0.0f;

    void Start()
    {
        gm = GameManager.Instance;
        sceneManager = SceneManager.Instance;
        LastIdleTime = Time.time;
        StartCoroutine(IdleCheck());
    }

    void Update()
    {
        if(gm.currMode != CurrentMode.StartMenu)
        {
            idleTime += Time.deltaTime;
            if(Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
            {
                idleTime = 0f;
            }
        }
    }

    IEnumerator IdleCheck()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            if(gm.currMode == CurrentMode.StartMenu) continue;

            if(idleTime > maximumIdleTime) sceneManager.ActivateStartMenu(); 
        }
    }
}
