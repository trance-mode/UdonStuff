using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class RandomSpawnObject : UdonSharpBehaviour 
{
    [Space(20)]
	[Header("-----------------------------")]
	[Header("切り替えるオブジェクト")]
	[Header("-----------------------------")]
	[Space(20)]
    [SerializeField] GameObject objectToToggle;
    [SerializeField] BoxCollider objectToToggleCollider;
    [SerializeField] MeshRenderer objectToToggleMeshRenderer;
    [SerializeField] Rigidbody objectToToggleRigidbody;

    [Space(20)]
	[Header("-----------------------------")]
	[Header("オブジェクトオンオフ切り替えフラグ")]
	[Header("-----------------------------")]
	[Space(20)]
    [SerializeField] bool isObjectActive = false;

    [Space(20)]
	[Header("-----------------------------")]
	[Header("時間間隔")]
	[Header("-----------------------------")]
	[Space(20)]
    [SerializeField] float durationTimeSet1 = 3f;

    [Space(20)]
	[Header("-----------------------------")]
	[Header("初期スポーン時の追加時間間隔")]
	[Header("-----------------------------")]
	[Space(20)]
    [SerializeField] float durationTimeSet2 = 5f;
    [SerializeField] float timeInterval = 5f; 
    [SerializeField] float timer = 0f;

    [SerializeField] AudioSource audioSource_Spawn;
    [SerializeField] AudioClip sound_Spawn;
    [SerializeField] AudioSource audioSource_Despawn;
    [SerializeField] AudioClip sound_Despawn;
    [SerializeField] float maxAudioDistance;

    [SerializeField] Transform spawnPoint;

    [SerializeField] ParticleSystem particleSpawn;
    [SerializeField] ParticleSystem particleDespawn;
    [HideInInspector] public VRCPlayerApi localPlayer;

    void OnEnable() 
    {
        timeInterval = durationTimeSet1 + durationTimeSet2;
        timer = 0f;
    }
    void Start() 
    {
        duractionTimeReset();
        localPlayer = Networking.LocalPlayer;
    }

    void Update() 
    {
        timer += Time.deltaTime; 
        if (timer >= timeInterval) { 
            isObjectActive = !isObjectActive; 
            if(isObjectActive) ItemOn();
            else ItemOff();
            duractionTimeReset();
        } 
    } 
    public void ItemOn() 
    {
        objectToToggle.transform.position = spawnPoint.position;
        //範囲外のプレイヤーに一瞬音が鳴るバグを回避
        float playerDistance = (localPlayer.GetPosition() - audioSource_Spawn.transform.position).sqrMagnitude;
        //float playerDistance = Vector3.Distance(Networking.LocalPlayer.GetPosition(), audioSource_Spawn_Obj.transform.position);
        if (playerDistance < maxAudioDistance) {
            audioSource_Spawn.Play();
        }

        particleSpawn.Play();
        objectToToggleCollider.enabled = true;
        objectToToggleMeshRenderer.enabled = true;
        objectToToggleRigidbody.isKinematic = false;
    }
    public void ItemOff() 
    {
        //範囲外のプレイヤーに一瞬音が鳴るバグを回避
        float playerDistance = (localPlayer.GetPosition() - audioSource_Despawn.transform.position).sqrMagnitude;
        //float playerDistance = Vector3.Distance(Networking.LocalPlayer.GetPosition(), audioSource_Despawn_Obj.transform.position);
        if (playerDistance < maxAudioDistance * maxAudioDistance) {
            audioSource_Despawn.Play();
        }

        particleDespawn.Play();
        objectToToggleCollider.enabled = false;
        objectToToggleMeshRenderer.enabled = false;
        objectToToggleRigidbody.isKinematic = true;
    }
    public void duractionTimeReset() 
    {
        timeInterval = durationTimeSet1;
        timer = 0f;
    }
}