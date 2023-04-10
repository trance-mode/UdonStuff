
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class NPCHeadHitCollider : UdonSharpBehaviour
{
    public NPCController4 npcController4;
    public void OnParticleCollision (GameObject other) 
    {
        if (other.gameObject.layer == 24) {
            if(Networking.IsOwner(other.gameObject)) {
                //見失うか確認フラグを下ろす
                SendCustomNetworkEvent(NetworkEventTarget.All, "CheckLostPlayerFalse");
                //見失ったと判定
                SendCustomNetworkEvent(NetworkEventTarget.All, "PlayerHitFalse");
            } 
        }
    }
}
