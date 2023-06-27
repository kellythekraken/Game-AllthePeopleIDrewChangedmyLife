using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarRotate : MonoBehaviour
{
    [SerializeField] Transform rotateTarget, lookCamera;

    internal bool wardrobeMode = false;
    Slider _slider;
    Vector3 newRotation;
    float startingRotation = 0f;

    void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(RotateAvatar);
    }
    void OnEnable()
    {
        ResetAvatar();
        _slider.value = _slider.minValue = startingRotation;
        _slider.maxValue = startingRotation + 360;
    }
    void OnDisable()
    {
        rotateTarget.localRotation = Quaternion.identity;
    }
    void ResetAvatar()
    {
        rotateTarget.LookAt(lookCamera);
        startingRotation = rotateTarget.eulerAngles.y;
        newRotation = rotateTarget.eulerAngles;
        newRotation.x = newRotation.z = 0;
        rotateTarget.eulerAngles = newRotation;
    }
    void RotateAvatar(float value)
    {
        newRotation = rotateTarget.eulerAngles;
        newRotation.y = value;
        rotateTarget.eulerAngles = newRotation;
    }

}
