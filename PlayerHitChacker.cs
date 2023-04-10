
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerHitChacker : UdonSharpBehaviour
{
    public NPCController4 npc; 
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.playerId != npc.targetPlayerId) return;
        npc.SendCustomEvent("PF"); 
        transform.position = transform.position + new Vector3(0, 500f, 0);   
    }
    private void OnTriggerEnter(Collider other) 
    {
        npc.SendCustomEvent("CPL");   
        transform.position = transform.position + new Vector3(0, 500f, 0);            
    }
}
