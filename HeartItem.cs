
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class HeartItem : UdonSharpBehaviour
{
    [SerializeField] public GameObject[] npcs;
    [SerializeField] public NPCController4[] npcController4;
    void OnParticleCollision(GameObject other)
    {
        //当たったオブジェクトのオーナーじゃないなら処理しない
        if(!Networking.IsOwner(other)) return;
        Networking.SetOwner(Networking.GetOwner(other.gameObject), gameObject);
        //npcs.LengthはNPCの数
        for(int i = 0; i < npcs.Length; i++) {
            //NPCがアクティブなら
            if(npcs[i].activeSelf) {
                //まず既存のオーナーを通してliveFlagをオンにしてOwnerが変わらないように設定
                npcController4[i].SendCustomNetworkEvent(NetworkEventTarget.All, "LoveFlagSet");
                //次にNPCのオーナーを変更
                Networking.SetOwner(Networking.GetOwner(other.gameObject), npcs[i]);
                //最後にNPCのオーナーを通してliveFlagをオンにする
                npcController4[i].SetProgramVariable("targetPlayerId", Networking.GetOwner(other.gameObject).playerId);
                npcController4[i].SendCustomNetworkEvent(NetworkEventTarget.All, "LoveFlagSet");
            }
        }
    }
}
