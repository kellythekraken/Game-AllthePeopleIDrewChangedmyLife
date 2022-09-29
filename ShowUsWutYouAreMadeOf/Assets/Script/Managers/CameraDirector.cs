using UnityEngine;
using Yarn.Unity;


public class CameraDirector : MonoBehaviour
{
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
        Debug.Log(_animator);
    }

    public void SwitchCamera(string animName)
    {
        _animator.Play(animName);
    }
}
