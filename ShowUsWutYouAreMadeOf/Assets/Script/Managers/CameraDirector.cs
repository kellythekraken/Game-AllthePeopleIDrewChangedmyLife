using UnityEngine;
using Yarn.Unity;


public class CameraDirector : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        dialogueRunner = GameManager.Instance.dialogueRunner;
        dialogueRunner.AddCommandHandler<string>("camera", SwitchCamera);

    }

    public void SwitchCamera(string animName)
    {
        _animator.Play(animName);
    }
}
