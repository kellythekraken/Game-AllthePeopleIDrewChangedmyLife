using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCManager : MonoBehaviour
{
    //Control the onstage and offstage of npc, keep track of the active and completed character,
    //and provide method to spawn random available npc.

    public static NPCManager Instance;
    [SerializeField] private QueerNPC[] NPCFullList;//add in the editor
    internal List<QueerNPC> NPCToSpawn;
    internal List<QueerNPC> completedNPC; //finished sketching and chatting
    private List<QueerNPC> activeNPC; //currently in scene
    //would there be one npc to stay until the end???

    void Awake() => Instance = this;
    void Start()
    {
        GameManager.Instance.dialogueRunner.AddCommandHandler("randomEnter", OnStageRandom);
        GameManager.Instance.dialogueRunner.AddCommandHandler<string>("enter", OnStage);
        GameManager.Instance.dialogueRunner.AddCommandHandler<string>("leave", OffStage);

        activeNPC = new List<QueerNPC>();
        completedNPC = new List<QueerNPC>();
        NPCToSpawn = new List<QueerNPC>(NPCFullList.ToList());
    }

    public QueerNPC FindNPC(string name)
    {
        return System.Array.Find(NPCFullList,t=>t.queerID.npcName == name);
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
        QueerNPC npc = NPCToSpawn.Find(x => x.queerID.npcName == npcName);
        npc.HideAndDisable(false);
        activeNPC.Add(npc);
        //NPCToSpawn.Remove(npc);
    }
    
    //command: leave
    public void OffStage(string npcName)
    {
        QueerNPC npc = activeNPC.Find(x => x.queerID.npcName == npcName);
        npc.HideAndDisable(true);

        activeNPC.Remove(npc);
        //completedNPC.Add(npc);
    }
}
