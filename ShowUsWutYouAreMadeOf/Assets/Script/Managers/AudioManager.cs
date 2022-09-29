using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();

    }
}
