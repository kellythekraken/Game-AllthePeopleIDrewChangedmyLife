
using UnityEngine;
using UnityEngine.InputSystem;

public class PointerFollower : MonoBehaviour
{
    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        Cursor.visible = false;
    }
    void OnDisable()
    {
        Cursor.visible = true;
    }
    void Update()
    {   
       Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
       transform.position = mousePosition;
    }
}
