using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field:SerializeField] public EventReference bookOpen {get ; private set;}
    [field:SerializeField] public EventReference bookClose {get ; private set;}

    public static FMODEvents Instance { get; private set;}


    void Awake()
    {
        Instance = this;
    }


}
