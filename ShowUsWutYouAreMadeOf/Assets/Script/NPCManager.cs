using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private QueerNPC[] NPCFullList;//add in the editor
    internal List<QueerNPC> NPCToSpawn;
    internal List<QueerNPC> completedNPC; //finished sketching and chatting
    private List<QueerNPC> activeNPC; //currently in scene
    //would there be one npc to stay until the end???

    void Start()
    {
        activeNPC = new List<QueerNPC>();
        completedNPC = new List<QueerNPC>();
        NPCToSpawn = new List<QueerNPC>(NPCFullList.ToList());
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
        QueerNPC npc = NPCToSpawn.Find(x => x.queerID.npcName == npcName);
        npc.gameObject.SetActive(true);
        activeNPC.Add(npc);
        NPCToSpawn.Remove(npc);
    }
    //command: leave
    public void OffStage(string npcName)
    {
        QueerNPC npc = NPCToSpawn.Find(x => x.queerID.npcName == npcName);
        Destroy(npc.gameObject);
        completedNPC.Add(npc);
        activeNPC.Remove(npc);
    }
}
