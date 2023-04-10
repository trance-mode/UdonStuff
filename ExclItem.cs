
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ExclItem : UdonSharpBehaviour
{
    [SerializeField] ParticleSystem[] particle;
    [SerializeField] int ranNum;
    void OnEnable() 
    {
        if(Networking.IsOwner(gameObject)) {
            //アイテム選び
            ranNum = Random.Range(0, particle.Length);
        } 
    }
    public void OnParticleCollision(GameObject other) 
    {
        if (other.gameObject.layer == 24) {
            if(Networking.IsOwner(other.gameObject)) {
                //アイテム選び
                ranNum = Random.Range(0, particle.Length);
                if (ranNum == 0) {
                    SendCustomNetworkEvent(NetworkEventTarget.All, nameof(LightParticleOn));
                }
                else {
                    SendCustomNetworkEvent(NetworkEventTarget.All, nameof(DarkParticleOn));
                }
            } 
        }
    }
    public void LightParticleOn() 
    {
        particle[0].transform.position = transform.position;
        particle[0].Play();
    }
    public void DarkParticleOn() 
    {
        particle[1].transform.position = transform.position;
        particle[1].Play();
    }
}
