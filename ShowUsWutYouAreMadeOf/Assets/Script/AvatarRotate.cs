using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarRotate : MonoBehaviour
{
    [SerializeField] Transform rotateTarget;
    internal bool wardrobeMode = false;
    Slider _slider;
    Vector3 newRotation;

    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.minValue = 0;
        _slider.maxValue = 360;
        _slider.onValueChanged.AddListener(RotateAvatar);
        newRotation = rotateTarget.eulerAngles;
    }
    void OnEnable()
    {
        ResetAvatar();
        _slider.value = 0;
    }
    void ResetAvatar()
    {
        rotateTarget.eulerAngles = Vector3.zero;
    }
    void RotateAvatar(float value)
    {
        newRotation.y = value;
        rotateTarget.eulerAngles = newRotation;
    }

}
