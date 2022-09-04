using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// YOU WILL NOT HAVE TO CHANGE THIS FILE
// this file is attached to the parent Location object of markers in the Scene 
// and its functions only exist for functions in SceneDirector.cs and Character.cs 
// to call them
public class Location : MonoBehaviour {
    public Transform GetMarkerWithName(string markerName) {
        Debug.Log("get marker is called");
        Transform marker = transform.Find(markerName);
        if (marker == null) {
             Debug.LogError($"Location {name} has no marker named {markerName}.");
            return null;           
        }
        return marker;
    }
}
