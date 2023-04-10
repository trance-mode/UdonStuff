
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
public class Item : UdonSharpBehaviour
{
	[Header("-----------------------------スポーン時とアイテム取得時の音設定-----------------------------")]
    [SerializeField] AudioSource audioSource_Spawn;
    [SerializeField] AudioClip spawnSound;
    [SerializeField] AudioSource audioSource_Hit;
    [SerializeField] AudioClip hitSound;

    [SerializeField] float maxAudioDistance;

    public UdonSharpBehaviour itemSpawnScript;

    [HideInInspector] public VRCPlayerApi localPlayer;
    void OnEnable() 
    {
        audioSource_Spawn.transform.position = gameObject.transform.position;
        audioSource_Spawn.clip = spawnSound;
        //範囲外のプレイヤーに一瞬音が鳴るバグを回避
        if(localPlayer == null) {
            localPlayer = Networking.LocalPlayer;
        }
        float playerDistance = (localPlayer.GetPosition() - audioSource_Spawn.transform.position).sqrMagnitude;
        //float playerDistance = Vector3.Distance(Networking.LocalPlayer.GetPosition(), audioSource_Spawn_Obj.transform.position);
        if (playerDistance < maxAudioDistance)
        {
            audioSource_Spawn.Play();
        }
    }
    void Start() 
    {
        localPlayer = Networking.LocalPlayer;
    }
    public void OnParticleCollision(GameObject other) 
    {
        if (other.gameObject.layer == 24) {
            if(Networking.IsOwner(gameObject)) {
                itemSpawnScript.SendCustomNetworkEvent(NetworkEventTarget.Owner, "ItemSpawn");
            }
            gameObject.SetActive(false);
        } 
    }
    void OnDisable() 
    {
        if(localPlayer == null) {
            return;
        }
        audioSource_Hit.transform.position = gameObject.transform.position;
        audioSource_Hit.clip = hitSound;
        //範囲外のプレイヤーに一瞬音が鳴るバグを回避
        float playerDistance = (localPlayer.GetPosition() - audioSource_Hit.transform.position).sqrMagnitude;
        //float playerDistance = Vector3.Distance(Networking.LocalPlayer.GetPosition(), audioSource_Hit_Obj.transform.position);
        if (playerDistance < maxAudioDistance * maxAudioDistance)
        {
            audioSource_Hit.Play();
        }
    }
}
