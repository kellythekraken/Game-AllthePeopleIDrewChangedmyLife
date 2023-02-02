using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    AudioSource source;
    //start with one music. For the restarted version always randomize music.
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        source = GetComponent<AudioSource>();
    }



}
