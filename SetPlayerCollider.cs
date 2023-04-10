
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class SetPlayerCollider : UdonSharpBehaviour
{
    public GameObject[] playerColliderSet;
    [SerializeField] CapsuleCollider[] p2N_SphereCol;
    [SerializeField] MeshRenderer[] p2N_SphereMesh;
    [SerializeField] CapsuleCollider[] p2N_SphereCol2;
    [SerializeField] MeshRenderer[] p2N_SphereMesh2;
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {   
        for(int i = 0; i < playerColliderSet.Length; i++) {
            if(Networking.GetOwner(playerColliderSet[i]) == player) {
                AttachCol(i);
            }
        }
    }
    public void AttachCol(int num) 
    {
        p2N_SphereCol[num].enabled = true;
        p2N_SphereMesh[num].enabled = true;
        p2N_SphereCol2[num].enabled = true;
        p2N_SphereMesh2[num].enabled = true;
    }
}
