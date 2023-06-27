using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Yarn.Unity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get;private set;}
    private List<EventInstance> eventList;
    Vector3 playerPosition;
    DialogueRunner dialogueRunner;
    EventInstance bgmEventInstance;

    //start with one music. For the restarted version always randomize music.
    void Awake() => Instance = this;
    void Start()
    {
        dialogueRunner = GameManager.Instance.dialogueRunner;
        eventList = new List<EventInstance>();
        playerPosition = GameManager.Instance.playerObject.transform.position;
        InitBGM(FMODEvents.Instance.music);
        //dialogueRunner.onNodeStart.AddListener(StartTyping);
        //dialogueRunner.onDialogueComplete.AddListener(StopTyping);
    }

    void InitBGM(EventReference reference)
    {
        bgmEventInstance = CreateEventInstance(reference);
        //bgmEventInstance.set3DAttributes();
        bgmEventInstance.start();
    }

    public void PlayOneShot(EventReference sound) //without distance
    {
        RuntimeManager.PlayOneShot(sound);
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos) //with distance
    {
        RuntimeManager.PlayOneShot(sound,worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventRef)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventRef);
        eventList.Add(eventInstance);
        return eventInstance;
    }
    float currentMuffle = 0f;
    public void SetMuffleParameter(float newValue)
    {
        if(currentMuffle!=newValue)
        {
            RuntimeManager.StudioSystem.setParameterByName("Muffle",newValue);
            currentMuffle = newValue;
        }
    }

    public void SetSceneParam(float newValue)
    {
        RuntimeManager.StudioSystem.setParameterByName("Scene",newValue);
    }

    void StartTyping(string arg0)
    {
        Debug.Log("start playing typing sound");
    }

    void StopTyping()
    {
        Debug.Log("stop playing typing sound");
    }

    private void CleanUp()
    {
        bgmEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        bgmEventInstance.release();

        foreach(var i in eventList)
        {
            i.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            i.release();
        }
    }

    void OnDestroy() => CleanUp();
}
