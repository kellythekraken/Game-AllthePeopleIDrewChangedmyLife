using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using Cinemachine;
public class PlayerRotation : MonoBehaviour
{
    public CinemachineFreeLook playerCam;
    CharacterController myCharacterController = null;
    public float walkSpeed = 10.0f;
    public float horizontalInput, verticalInput;

    public float speed = 6.0F;
    // Drag & Drop the camera in this field, in the inspector
    public Transform cameraTransform ;
    private Vector3 moveDirection = Vector3.zero;
    void Update() {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = cameraTransform.TransformDirection(moveDirection);
            moveDirection *= speed;
        }

        controller.Move(moveDirection * Time.deltaTime);
    }
    /*    void Start()
        {
            myCharacterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            // Get Horizontal and Vertical Input
            horizontalInput = Input.GetAxis("Player1_Horizontal");
            verticalInput = Input.GetAxis("Player1_Vertical");

            // Calculate the Direction to Move based on the tranform of the Player


            //find the direction
            Vector3 moveDirectionForward = playerCam.transform.forward * verticalInput * Time.deltaTime;
            Vector3 moveDirectionSide = transform.right * horizontalInput * Time.deltaTime;
            //(moveDirectionForward + moveDirectionSide).normalized;
            //find the distance
            Vector3 distance = moveDirectionForward * walkSpeed * Time.deltaTime;

            // Apply Movement to Player
            myCharacterController.Move(distance);

            transform.position = transform.position + playerCam.transform.forward * speed * Time.deltaTime * verticalInput;
        }

        public virtual Vector2 ApplyCameraRotation(Vector2 input)
        {

            var _cameraAngle = playerCam.transform.localEulerAngles.y;
            return MMMaths.RotateVector2(input, -_cameraAngle);

        }*/
}
