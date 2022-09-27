using UnityEngine;
using Yarn.Unity;


public class CameraDirector : MonoBehaviour
{
    public Camera playerCam, stateCam;
    private DialogueRunner dialogueRunner;
    private Animator _animator;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        dialogueRunner.AddCommandHandler<string>("camera", SwitchCamera);
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayerCam()
    {

    }

    public void RestCam()
    {
        
    }
    public void SwitchCamera(string animName)
    {
        _animator.Play(animName);
    }
}
