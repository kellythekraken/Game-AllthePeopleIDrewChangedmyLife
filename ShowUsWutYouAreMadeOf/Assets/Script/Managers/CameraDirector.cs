using UnityEngine;
using Yarn.Unity;
using Cinemachine;

public class CameraDirector : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private CinemachineStateDrivenCamera stateCam;
    private DialogueRunner dialogueRunner;
    private Animator _animator;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        dialogueRunner.AddCommandHandler<string>("camera", SwitchCamera);
        dialogueRunner.AddCommandHandler("playercam", SwitchToPlayerCam);

    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SwitchToPlayerCam()
    {
        playerCam.Priority = 10; stateCam.Priority = 8;
    }

    public void SwitchCamera(string animName)
    {
        playerCam.Priority = 8; stateCam.Priority = 10;
        _animator.Play(animName);
    }
}
