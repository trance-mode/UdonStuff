
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class P2PColliderEnterChecker : UdonSharpBehaviour
{
    [Space(20)]
	[Header("-----------------------------誰がOwnerかを表示するText-----------------------------")]
    [Space(20)]
    [SerializeField] private float playerRunSpeed = 4.0f;
    [SerializeField] private float playerWalkSpeed = 2.0f;
    [SerializeField] private float playerStrafeSpeed = 2.0f;
    [SerializeField] private float playerJumpImpulse = 3.0f;
    [SerializeField] private float playerGravityStrength = 1.0f;
    [Space(20)]
	[Header("-----------------------------誰がOwnerかを表示するText-----------------------------")]
    [Space(20)]
    [SerializeField] GameObject iceParticle;
    [SerializeField] AudioSource p2P_IceSound;
    [Space(20)]
	[Header("-----------------------------誰がOwnerかを表示するText-----------------------------")]
    [Space(20)]
    [SerializeField] CapsuleCollider p2N_SphereCol;
    [SerializeField] MeshRenderer p2N_SphereMesh;
    [SerializeField] CapsuleCollider p2N_SphereCol2;
    [SerializeField] MeshRenderer p2N_SphereMesh2;
    [SerializeField] CapsuleCollider p2p_SphereCol;
    [SerializeField] MeshRenderer p2p_SphereMesh;
    [Space(20)]
	[Header("-----------------------------誰がOwnerかを表示するText-----------------------------")]
    [Space(20)]
    [SerializeField] GameObject playerColliderController;
    //"user0"FindPlayerSphereCollider_LayerName
    //"user1"PlayerHitterCollider_LayerName
    //"user2"PlayerCollider_LayerName
    //"user3"NPCCollider_LayerName
    //"user4"P2PCollider_LayerName
    [Space(20)]
	[Header("-----------------------------誰がOwnerかを表示するText-----------------------------")]
    [Space(20)]
    public JudgeCounter _JudgeCounter;
    [Space(20)]
	[Header("-----------------------------誰がOwnerかを表示するText-----------------------------")]
    [Space(20)]
    [SerializeField] private float countSecondTime = 0;
    [SerializeField] private float countSecondTimeSet = 1f;
    [SerializeField] bool countSecond = false;

    [Space(20)]
	[Header("-----------------------------誰がOwnerかを表示するText-----------------------------")]
    [Space(20)]
    [SerializeField] TextMeshProUGUI OptionText;

    [Space(20)]
	[Header("-----------------------------誰がOwnerかを表示するText-----------------------------")]
    [Space(20)]
    public HealShot healShot;
    public Pillow_Item pillow_Item;
    
    private void Start()
    {
        //オブジェクトのオーナーを表示させる。
        SetOptionalText(Networking.LocalPlayer);
    }
    void Update()
    {
        if (countSecond == true) {
            countSecondTime -= Time.deltaTime;
            if (countSecondTime <= 0) {
                countSecondTime = countSecondTimeSet;
                countSecond = false;
            }
        }
        else if (p2p_SphereCol.enabled == false) {
            countSecond = false;
            countSecondTime = 0;
        }
    }
    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        SetOptionalText(Networking.LocalPlayer);
    }
    // 対象オブジェクトのオーナーを表示させる。
    public void SetOptionalText(VRCPlayerApi player)
    {
        if(OptionText != null) OptionText.text = $"<color=red>{player.displayName} is Owner!</color>";
    }
    private void OnParticleCollision(GameObject other) 
    {
        //このオブジェクトのオーナーではない場合は以下の処理をしない
        if(!Networking.IsOwner(gameObject)) return;
        VRCPlayerApi player = Networking.LocalPlayer;
        //星落としパーティクルに当たった場合エフェクトを出しプレイヤーの速度をデフォルトに戻し、カウンターを増やす
        if (other.name == "StarFallPS") {
            _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CA"));
            SetSpeed();
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(IOf));
        }
        else if (other.name == "HealPS") {
            _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CA"));
            SetSpeed();
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(IOf));
        }
        //ビックリマーク（光）に当たった場合エフェクトを出しプレイヤーの速度をデフォルトに戻し、カウンターを増やす
        else if (other.name == "LightPS") {
            _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CA"));
            SetSpeed();
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(IOf));
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(!Networking.IsOwner(gameObject)) return;
        //user2//playercollide
        if (other.gameObject.layer == 24) {
            if (!countSecond) {
                countSecond = true;
                _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CA"));
                SetSpeed();
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(IOf));
            }
        }
        //user5
        if (other.gameObject.layer == 27) {
            SetSpeed();
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(EF));
        }
    }
    public void SetSpeed() 
    {
        VRCPlayerApi player = Networking.LocalPlayer;
        player.SetRunSpeed(playerRunSpeed);
        player.SetWalkSpeed(playerWalkSpeed);
        player.SetStrafeSpeed(playerStrafeSpeed);
        player.SetJumpImpulse(playerJumpImpulse);
        player.SetGravityStrength(playerGravityStrength);
    }
    public void EF()
    {
        p2N_SphereCol.enabled = false;
        p2N_SphereMesh.enabled = false;
        p2N_SphereCol2.enabled = false;
        p2N_SphereMesh2.enabled = false;
        p2p_SphereCol.enabled = false;
        p2p_SphereMesh.enabled = false;
        iceParticle.SetActive(false);
        p2P_IceSound.Play();
    }
    public void IOf()
    {
        p2N_SphereCol.enabled = true;
        p2N_SphereMesh.enabled = true;
        p2N_SphereCol2.enabled = true;
        p2N_SphereMesh2.enabled = true;
        p2p_SphereCol.enabled = false;
        p2p_SphereMesh.enabled = false;
        iceParticle.SetActive(false);
        p2P_IceSound.Play();
    }
}
