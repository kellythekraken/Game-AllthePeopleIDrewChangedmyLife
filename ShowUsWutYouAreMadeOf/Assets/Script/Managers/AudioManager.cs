using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get;private set;}
    private List<EventInstance> eventList;
    Vector3 playerPosition;

    //start with one music. For the restarted version always randomize music.
    void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("warning, more than one audio manager");
        }
        Instance = this;
    }

    void Start()
    {
        eventList = new List<EventInstance>();
        playerPosition = GameManager.Instance.playerObject.transform.position;
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

    private void CleanUp()
    {
        foreach(var i in eventList)
        {
            i.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            i.release();
        }
    }

    void OnDestroy() => CleanUp();
}
