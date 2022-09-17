
using UnityEngine;
using UnityEngine.InputSystem;

public class PointerFollower : MonoBehaviour
{
    Camera mainCamera;
    Vector2 offset = new Vector2(50f,45f);
    void OnEnable()
    {
        mainCamera = Camera.main;
        //Cursor.visible = false;
    }
    void OnDisable()
    {
        Cursor.visible = true;
    }
    void Update()
    {   
       transform.position = Mouse.current.position.ReadValue() + offset;
    }
}
