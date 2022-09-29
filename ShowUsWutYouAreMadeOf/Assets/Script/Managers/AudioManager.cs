using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource source;
    //start with one music. For the restarted version always randomize music.
    void Start()
    {
        source = GetComponent<AudioSource>();

    }
}
