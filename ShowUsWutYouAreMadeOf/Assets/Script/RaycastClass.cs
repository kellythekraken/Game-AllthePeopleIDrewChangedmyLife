using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastClass : MonoBehaviour
{
    //attached to inputmanager obj, when enabled, will send out raycast for sketching evaluation

    Camera mainCam;
    InputManager inputManager;
    Ray ray;
    RaycastHit hit;
    [SerializeField] LayerMask raycastHit;

    GameObject currentTarget,lastTarget;
    int bodypartLayer;
    Material selected; //if selected, it should stay highlighted

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        mainCam = Camera.main;
        bodypartLayer = LayerMask.NameToLayer("Bodypart");
    }
    void OnDisable()
    {
        currentTarget = lastTarget = null;
    }
    void Update()
    {
         ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
         if(Physics.Raycast(ray, out hit, 40f))//, raycastHit
         {
            var hitTarget = hit.transform.gameObject;
            if(hitTarget.layer == bodypartLayer)
            {
                Debug.Log(hitTarget.name);
                if (lastTarget != hitTarget) HighlightMaterial(hitTarget);
            }
            else
            {
                UnlightMaterial(lastTarget);
                return;
            }
         }
     }

    void HighlightMaterial(GameObject target)
    {
        //highlight the target material, make the last one normal
        target.GetComponentInParent<SkinnedMeshRenderer>().material.color = Color.black;
        UnlightMaterial(lastTarget);
        lastTarget = target;
    }

    void UnlightMaterial(GameObject obj)
    {
        if(obj == null) return;
        obj.GetComponentInParent<SkinnedMeshRenderer>().material.color = Color.white;
    }
}
