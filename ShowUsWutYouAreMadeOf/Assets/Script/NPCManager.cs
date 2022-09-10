using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private GameObject[] NPCFullList;//add in the editor
    internal List<GameObject> NPCToSpawn;
    List<GameObject> activeNPC; //currently in scene
    internal List<GameObject> completedNPC; //finished sketching and chatting
    //would there be one npc to stay until the end???

    void Start()
    {
        activeNPC = new List<GameObject>();
        completedNPC = new List<GameObject>();
        NPCToSpawn = new List<GameObject>(NPCFullList.ToList());
    }
    //command: enter random
    public void OnStageRandom()
    {
        int num = Random.Range(0,NPCToSpawn.Count());
        OnStage(NPCToSpawn[num].name);
    }

    //command: enter
    public void OnStage(string npcName)
    {
        Debug.LogWarning(npcName + " enters the scene!");
        GameObject npc = NPCToSpawn.Find(x => x.name == npcName);
        npc.SetActive(true);
        activeNPC.Add(npc);
        NPCToSpawn.Remove(npc);
    }
    //command: leave
    public void OffStage(string npcName)
    {
        GameObject npc = NPCToSpawn.Find(x => x.name == npcName);
        Destroy(npc);
        completedNPC.Add(npc);
        activeNPC.Remove(npc);
    }
}
