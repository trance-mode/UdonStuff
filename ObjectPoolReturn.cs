
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class ObjectPoolReturn : UdonSharpBehaviour 
{
    [SerializeField] float countSecondTime;
    [SerializeField] float countSecondTimeSet = 10;
    [SerializeField] UCS.UdonChips udonChips;
    [Tooltip("サウンド")] 
    [SerializeField] AudioSource audioSource_SpawnCoin;
    [SerializeField] AudioClip audioClip_SpawnCoin;
    [SerializeField] AudioSource audioSource_HitCoin;
    [SerializeField] AudioClip audioClip_HitCoin;
    [SerializeField] AudioSource audioSource_DeSpawn;
    [SerializeField] AudioClip audioClip_DeSpawn;

    [SerializeField] float maxAudioDistance;

    [HideInInspector] public VRCPlayerApi localPlayer;

    [Tooltip( "入手金額" )]
    [SerializeField] int price = 100;
    void Enabled() 
    {
        countSecondTime = 0;
        audioSource_SpawnCoin.clip = audioClip_SpawnCoin;
    }
    void Start() 
    {
        localPlayer = Networking.LocalPlayer;
    }
    void Update() 
    {
        countSecondTime += Time.deltaTime;
        if (countSecondTime > countSecondTimeSet) {
            ResetCoin();
            audioSource_DeSpawn.transform.position = transform.position;
            audioSource_DeSpawn.clip = audioClip_DeSpawn;
            float playerDistance = (localPlayer.GetPosition() - audioSource_DeSpawn.transform.position).sqrMagnitude;
            //float playerDistance = Vector3.Distance(Networking.LocalPlayer.GetPosition(), audioSource_DeSpawn_Obj.transform.position);
            if (playerDistance < maxAudioDistance)
            {
                audioSource_DeSpawn.Play();
            }
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        //"user2"PlayerColliderLayerName
        if (other.gameObject.layer == 24) {
            ResetCoin();
            audioSource_HitCoin.transform.position = transform.position;
            audioSource_HitCoin.clip = audioClip_HitCoin; 
            //範囲外のプレイヤーに一瞬音が鳴るバグを回避
            float playerDistance = (localPlayer.GetPosition() - audioSource_HitCoin.transform.position).sqrMagnitude;
            //float playerDistance = Vector3.Distance(Networking.LocalPlayer.GetPosition(), audioSource_HitCoin_Obj.transform.position);
            if (playerDistance < maxAudioDistance * maxAudioDistance)
            {
                audioSource_HitCoin.Play();
            }
            if (!Networking.IsOwner(other.gameObject)) return;
            udonChips.money += price;
        }
        //"user5"
        else if (other.gameObject.layer == 27) {
            ResetCoin();
        }
    }
    public void ResetCoin() {
        countSecondTime = 0;
        gameObject.SetActive(false);
    }
}
