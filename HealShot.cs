
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
public class HealShot : UdonSharpBehaviour
{
    [Space(20)]
	[Header("----------------------------親オブジェクト----------------------------")]
	[Space(20)]
    [SerializeField] Transform originalParent;
    [SerializeField] Transform grandparent;
    
    [Space(20)]
	[Header("----------------------------アイテム----------------------------")]
	[Space(20)]
    [SerializeField] GameObject item_Particle;

    [Space(20)]
	[Header("----------------------------アイテムのコライダー----------------------------")]
	[Space(20)]
    [SerializeField] Collider item_Col;

    [Space(20)]
	[Header("----------------------------アイテムのパーティクル----------------------------")]
	[Space(20)]
    public ParticleSystem shot_Particle;

    [Space(20)]
	[Header("----------------------------音設定----------------------------")]
	[Space(20)]
    public AudioSource shot_Sound;

    [Space(20)]
	[Header("----------------------------弾設定----------------------------")]
	[Space(20)]
    public int bullet = 1;
    public int bullet_Set = 1;

    [Space(20)]
	[Header("----------------------------誰がOwnerかを表示するText----------------------------")]
    [Space(20)]
    [SerializeField] TextMeshProUGUI OptionText;
    
    private void Start()
    {
        //オブジェクトのオーナーを表示させる。
        SetOptionalText(Networking.LocalPlayer);
    }
    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        SetOptionalText(Networking.LocalPlayer);
    }

    // 対象オブジェクトのオーナーを表示させる。
    public void SetOptionalText(VRCPlayerApi player)
    {
        if(OptionText != null) OptionText.text = $"<color=red>{player.playerId} is Owner!</color>";
    }
    public override void OnPickup()
    {
        if(Networking.IsOwner(gameObject)) {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(MoveToGrandparent));
        }
    }

    public void MoveToGrandparent()
    {
        // 2つ上の親オブジェクトの子オブジェクトにする
        transform.SetParent(grandparent);
    }

    public void MoveToOriginalParent()
    {
        // 元の親オブジェクトの子オブジェクトにする
        transform.SetParent(originalParent);
    }

    public override void OnPickupUseDown()
    {
        if(Networking.IsOwner(gameObject)) {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(Shot));
        }
    }

    public void Shot()
    {
        item_Particle.SetActive(false);
        item_Col.enabled = false;
        if(bullet > 0) {
            bullet--;
            shot_Particle.Play();
            shot_Sound.Play();
        }
        bullet = bullet_Set;
        //ピックアップから強制的に手を離す
        VRC_Pickup pickup = (VRC_Pickup)this.gameObject.GetComponent(typeof(VRC_Pickup));
        pickup.Drop();
        MoveToOriginalParent();
    }

    public void GameSet()
    {
        item_Particle.SetActive(false);
        item_Col.enabled = false;
        bullet = bullet_Set;
        //ピックアップから強制的に手を離す
        VRC_Pickup pickup = (VRC_Pickup)this.gameObject.GetComponent(typeof(VRC_Pickup));
        pickup.Drop();
        MoveToOriginalParent();
    }
}