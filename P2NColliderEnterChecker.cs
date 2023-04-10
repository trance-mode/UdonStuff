
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class P2NColliderEnterChecker : UdonSharpBehaviour
{
    [HideInInspector] public VRCPlayerApi localPlayer;
    [Space(20)]
	[Header("-----------------------------各プレイヤーの当たり判定コライダー設定-----------------------------")]
	[Space(20)]
    [SerializeField] GameObject playerColliderSet;
    [SerializeField] CapsuleCollider p2N_SphereCol;
    [SerializeField] MeshRenderer p2N_SphereMesh;
    [SerializeField] CapsuleCollider p2N_SphereCol2;
    [SerializeField] MeshRenderer p2N_SphereMesh2;
    [SerializeField] CapsuleCollider p2P_SphereCol;
    [SerializeField] MeshRenderer p2P_SphereMesh;

    [Space(20)]
	[Header("-----------------------------プレイヤーのデフォルト速度設定-----------------------------")]
	[Space(20)]
    [SerializeField] private float playerdefRunSpeed = 4.0f;
    [SerializeField] private float playerdefWalkSpeed = 2.0f;
    [SerializeField] private float playerdefStrafeSpeed = 2.0f;
    [SerializeField] private float playerdefJumpImpulse = 3.0f;
    [SerializeField] private float playerdefGravityStrength = 1.0f;

    [Space(20)]
	[Header("-----------------------------プレイヤーが氷漬けにされたときの速度設定-----------------------------")]
	[Space(20)]
    [SerializeField] private float iceDurRunSpeed = 0.0f;
    [SerializeField] private float iceDurWalkSpeed = 0.0f;
    [SerializeField] private float iceDurStrafeSpeed = 0.0f;
    [SerializeField] private float iceDurJumpImpulse = 0.0f;
    [SerializeField] private float iceDurGravityStrength = 0.0f;

    [Space(20)]
	[Header("-----------------------------氷漬けエフェクト-----------------------------")]
	[Space(20)]
    [SerializeField] GameObject iceParticle;
    [SerializeField] AudioSource p2N_IceSound;

    [Space(20)]
	[Header("-----------------------------プレイヤーがレーザー当たった時の速度設定-----------------------------")]
	[Space(20)]
    [SerializeField] private float laserDurRunSpeed = 0.1f;
    [SerializeField] private float laserDurWalkSpeed = 0.1f;
    [SerializeField] private float laserDurStrafeSpeed = 0.1f;
    [SerializeField] private float laserDurJumpImpulse = 0.1f;
    [SerializeField] private float laserDurGravityStrength = 10.0f;
    [Space(20)]
	[Header("-----------------------------レーザー効果の持続時間-----------------------------")]
	[Space(20)]
    [SerializeField] private float laser_DurTime_Set = 5f;
    [SerializeField] private float laser_DurTime = 0f;
    [Space(20)]
	[Header("-----------------------------レーザーに当たったかどうか判定するフラグ-----------------------------")]
	[Space(20)]
    [SerializeField] private bool laserHit = false;

    [Space(20)]
	[Header("-----------------------------プレイヤーレーザーエフェクト-----------------------------")]
	[Space(20)]
    [SerializeField] GameObject laserParticle;
    [SerializeField] AudioSource laserSound;

    [Space(20)]
	[Header("-----------------------------プレイヤー骸骨マークエフェクト-----------------------------")]
	[Space(20)]
    [SerializeField] ParticleSystem skullParticle;

    [Space(20)]
	[Header("-----------------------------プレイヤー爆弾エフェクト-----------------------------")]
	[Space(20)]
    [SerializeField] ParticleSystem bombsParticle;
    [SerializeField] AudioSource bombsSound;

    [Space(20)]
	[Header("-----------------------------プレイヤー爆弾エフェクト-----------------------------")]
	[Space(20)]
    [SerializeField] ParticleSystem darkParticle;
    [SerializeField] AudioSource darkSound;

    [Space(20)]
	[Header("-----------------------------プレイヤーがスピードアップアイテムを取った時の速度設定-----------------------------")]
	[Space(20)]
    [SerializeField] private float durSpeedUPItem_RunSpeed = 6.0f;

    [Space(20)]
	[Header("-----------------------------スピードアップアイテムの持続時間-----------------------------")]
	[Space(20)]
    [SerializeField] private float speedUPItem_DurTime_Set = 5f;
    [SerializeField] private float speedUPItem_DurTime = 0f;

    [Space(20)]
	[Header("-----------------------------スピードアップアイテムを取ったかどうか判定するフラグ-----------------------------")]
	[Space(20)]
    [SerializeField] private bool speedUPItemGet = false;

    [Space(20)]
	[Header("-----------------------------プレイヤースピードアップアイテムエフェクト-----------------------------")]
	[Space(20)]
    [SerializeField] GameObject speedUpParticle;

    [Space(20)]
	[Header("-----------------------------スターアイテムの持続時間-----------------------------")]
	[Space(20)]
    [SerializeField] private float starItem_DurTime_Set = 5f;
    [SerializeField] private float starItem_DurTime = 0f;
    [Space(20)]
	[Header("-----------------------------スターアイテムを取ったか判定するフラグ-----------------------------")]
	[Space(20)]
    [SerializeField] private bool starItemGet = false;

    [Space(20)]
	[Header("-----------------------------プレイヤースターアイテムエフェクト-----------------------------")]
    [Space(20)]
    [SerializeField] GameObject starParticle;

    [Space(20)]
	[Header("-----------------------------枕効果の持続時間-----------------------------")]
	[Space(20)]
    [SerializeField] private float pillow_DurTime_Set = 5f;
    [SerializeField] private float pillow_DurTime = 0f;

    [Space(20)]
	[Header("-----------------------------枕に当たったかどうか判定するフラグ-----------------------------")]
	[Space(20)]
    [SerializeField] private bool pillowHit = false;

    [Space(20)]
	[Header("-----------------------------枕エフェクト-----------------------------")]
	[Space(20)]
    [SerializeField] GameObject pillowParticle;

    [Space(20)]
	[Header("-----------------------------弾アイテム-----------------------------")]
	[Space(20)]
    [SerializeField] private GameObject bulletItem;

    [Space(20)]
	[Header("-----------------------------弾アイテムのメッシュ-----------------------------")]
	[Space(20)]
    [SerializeField] MeshRenderer bulletItem_Mesh;

    [Space(20)]
	[Header("-----------------------------弾アイテムのコライダー-----------------------------")]
	[Space(20)]
    [SerializeField] Collider bulletItem_Col;

    [Space(20)]
	[Header("-----------------------------弾アイテムエフェクト-----------------------------")]
	[Space(20)]
    [SerializeField] GameObject bullet_Particle;

    [Space(20)]
	[Header("-----------------------------弾アイテムリスポーンポイント-----------------------------")]
	[Space(20)]
    [SerializeField] Transform bulletItemRespawnPoint;

    [Space(20)]
	[Header("-----------------------------ヒールアイテム-----------------------------")]
	[Space(20)]
    [SerializeField] private GameObject healItem;

    [Space(20)]
	[Header("-----------------------------ヒールアイテムのコライダー-----------------------------")]
	[Space(20)]
    [SerializeField] Collider healItem_Col;
    
    [Space(20)]
	[Header("-----------------------------ヒールアイテムエフェクト-----------------------------")]
	[Space(20)]
    [SerializeField] GameObject heal_Particle;

    [Space(20)]
	[Header("-----------------------------ヒールアイテムリスポーンポイント-----------------------------")]
	[Space(20)]
    [SerializeField] Transform healItemRespawnPoint;

    [Space(20)]
	[Header("-----------------------------ライトニングUIの持続時間-----------------------------")]
	[Space(20)]
    [SerializeField] private float lightningUI_DurTime_Set = 5f;
    [SerializeField] private float lightningUI_DurTime = 0f;
    [Space(20)]
	[Header("-----------------------------ライトニングアイテムを取ったか判定するフラグ-----------------------------")]
	[Space(20)]
    [SerializeField] private bool lightningItemGet = false;
    [Space(20)]
	[Header("-----------------------------")]
	[Header("-----------------------------星落としパーティクル-----------------------------")]
	[Header("-----------------------------")]
	[Space(20)]
    [SerializeField] private ParticleSystem starfallParticle;
    [Space(20)]
	[Header("-----------------------------星落とし警告UI-----------------------------")]
	[Space(20)]
    [SerializeField] private GameObject lightning_WarningUI;
    [Space(20)]
	[Header("-----------------------------星落とし警告音-----------------------------")]
	[Space(20)]
    [SerializeField] private AudioSource starfallSound;


    [Space(20)]
	[Header("-----------------------------銃弾がヒットした時のパーティクル-----------------------------")]
	[Space(20)]
    [SerializeField] private ParticleSystem blasterHitParticle;

    [Header("-----------------------------")]
	[Header("-----------------------------シールドアイテムの持続時間-----------------------------")]
	[Header("-----------------------------")]
	[Space(20)]
    [SerializeField] private float shieldItem_DurTime_Set = 5f;
    [SerializeField] private float shieldItem_DurTime = 0f;
    [Space(20)]
	[Header("-----------------------------シールドアイテムを取ったか判定するフラグ-----------------------------")]
	[Space(20)]
    [SerializeField] private bool shieldItemGet = false;
    [Space(20)]
	[Header("-----------------------------プレイヤーシールドアイテムエフェクト-----------------------------")]
    [Space(20)]
    [SerializeField] GameObject shieldParticle;

    [Space(20)]
	[Header("-----------------------------プレイヤーがゲームに参加しているかどうか判定するフラグ-----------------------------")]
    [Space(20)]
    [SerializeField] GameObject playerPlayingChecker;

    [Space(20)]
	[Header("-----------------------------誰がOwnerかを表示するText-----------------------------")]
    [Space(20)]
    [SerializeField] TextMeshProUGUI OptionText;

    [Space(20)]
	[Header("-----------------------------スクリプト-----------------------------")]
    [Space(20)]
    public HealShot healShot;
    public Pillow_Item pillow_Item;
    public JudgeCounter _JudgeCounter;

    private void Start()
    {
        localPlayer = Networking.LocalPlayer;
        //オブジェクトのオーナーを表示させる。
        SetOptionalText(localPlayer);
    }
    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        SetOptionalText(localPlayer);
    }
    // 対象オブジェクトのオーナーを表示させる。
    public void SetOptionalText(VRCPlayerApi player)
    {
        if(OptionText != null) OptionText.text = $"<color=red>{player.displayName} is Owner!</color>";
    }
    void Update()
    {
        if (speedUPItemGet) {
            speedUPItem_DurTime -= Time.deltaTime;
            if (speedUPItem_DurTime <= 0 || localPlayer.GetRunSpeed() <= 0.1f) {
                if (localPlayer.GetRunSpeed() > 0.1f && !starItemGet) {
                    SetDefSpeed();
                }
                speedUPItemGet = false;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SpeedUpEffectOff));
                speedUPItem_DurTime = speedUPItem_DurTime_Set;
            }
        }
        if (laserHit) {
            laser_DurTime -= Time.deltaTime * 2;
            if (laser_DurTime <= 0) {
                laserHit = false;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(LaserEffectOff));
                SetDefSpeed();
                laser_DurTime = laser_DurTime_Set;
            }
            else if (localPlayer.GetRunSpeed() == 0.0f) {
                laserHit = false;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(LaserEffectOff));
                laser_DurTime = laser_DurTime_Set;
            }
        }
        if (starItemGet) {
            starItem_DurTime -= Time.deltaTime;
            if (starItem_DurTime <= 0) {
                SetDefSpeed();
                starItemGet = false;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(StarEffectOff));
                starItem_DurTime = starItem_DurTime_Set;
            }
        }
        if (lightningItemGet) {
            lightningUI_DurTime -= Time.deltaTime;
            if (lightningUI_DurTime <= 0) {
                lightningItemGet = false;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(LightningUIOff));
                lightningUI_DurTime = lightningUI_DurTime_Set;
            }
        }
        if (pillowHit) {
            pillow_DurTime -= Time.deltaTime;
            if (pillow_DurTime <= 0) {
                SetDefSpeed();
                pillowHit = false;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(PillowEffectOff));
                pillow_DurTime = pillow_DurTime_Set;
            }
        }
        if (shieldItemGet) {
            shieldItem_DurTime -= Time.deltaTime;
            if (shieldItem_DurTime <= 0) {
                SetDefSpeed();
                shieldItemGet = false;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ShieldEffectOff));
                shieldItem_DurTime = shieldItem_DurTime_Set;
            }
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        //このオブジェクトのオーナーではない場合は以下の処理をしない
        if(!Networking.IsOwner(gameObject)) {
            return;
        }
        //case by case
        Vector3 vel;
        switch(other.name) {
            //スピードアップアイテムに当たった場合持続効果時間を設定しエフェクトを出しプレイヤーの速度を上げる
            case "SpeedUpPS": 
                //寝てる状態なら処理をしない
                if (localPlayer.GetRunSpeed() == 0.0f) return;
                speedUPItem_DurTime = speedUPItem_DurTime_Set;
                speedUPItemGet = true;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SpeedUpEffectOn));
                localPlayer.SetRunSpeed(durSpeedUPItem_RunSpeed);
                break;
            //LaserParticleSystemに当たった場合持続効果時間を設定しエフェクトを出しプレイヤーの速度を遅くする
            case "LaserPS":
                //すでに当たっている場合,寝てる状態は処理をしない
                if (laserHit == true || localPlayer.GetRunSpeed() == 0.0f) return;
                laser_DurTime = laser_DurTime_Set;
                laserHit = true;
                localPlayer.SetRunSpeed(laserDurRunSpeed);
                localPlayer.SetWalkSpeed(laserDurWalkSpeed);
                localPlayer.SetStrafeSpeed(laserDurStrafeSpeed);
                localPlayer.SetJumpImpulse(laserDurJumpImpulse);
                localPlayer.SetGravityStrength(laserDurGravityStrength);
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(LaserEffectOn));
                break;
            //PillowParticleSystemに当たった場合持続効果時間を設定しエフェクトを出しプレイヤーの速度を止める
            case "PillowPS":
                pillow_DurTime = pillow_DurTime_Set;
                pillowHit = true;
                localPlayer.SetRunSpeed(0f);
                localPlayer.SetWalkSpeed(0f);
                localPlayer.SetStrafeSpeed(0f);
                localPlayer.SetJumpImpulse(0f);
                localPlayer.SetGravityStrength(10.0f);
                vel = 0.0f * localPlayer.GetVelocity();
                localPlayer.SetVelocity(vel);
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(PillowEffectOn));
                healShot.SendCustomEvent("GameSet");
                pillow_Item.SendCustomEvent("GameSet");
                break;
            //BulletParticleSystemに当たった場合プレイヤーにアイテムを持たす
            case "BulletPS":
                //寝てる状態なら処理をしない
                if (localPlayer.GetRunSpeed() == 0.0f) return;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(BulletItemOn));
                break;
            
            case "HealthPS":
                //寝てる状態なら処理をしない
                if (localPlayer.GetRunSpeed() == 0.0f) return;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(HealItemOn));
                break;
            //SkullParticleに当たった場合エフェクトを出しプレイヤーの速度を止め、カウンターを減らし、カウンターが0になったか確認する
            case "SkullPS":
                localPlayer.SetRunSpeed(iceDurRunSpeed);
                localPlayer.SetWalkSpeed(iceDurWalkSpeed);
                localPlayer.SetStrafeSpeed(iceDurStrafeSpeed);
                localPlayer.SetJumpImpulse(iceDurJumpImpulse);
                localPlayer.SetGravityStrength(iceDurGravityStrength);
                vel = 0.0f * localPlayer.GetVelocity();
                localPlayer.SetVelocity(vel);
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SkullEffectOn));
                _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CL"));
                _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CC"));
                healShot.SendCustomEvent("GameSet");
                pillow_Item.SendCustomEvent("GameSet");
                break;
            //スターアイテムに当たった場合持続効果時間を設定しエフェクトを出しプレイヤーの速度を上げコライダーを消す
            case "StarPS":
                starItem_DurTime = starItem_DurTime_Set;
                starItemGet = true;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(StarEffectOn));
                localPlayer.SetRunSpeed(durSpeedUPItem_RunSpeed);
                break;
            //ライトニングアイテムに当たった場合星を落とすパーティクルを出し警告マークを表示させ警告マークを表示時間を設定
            case "LightningPS":
                lightningUI_DurTime = lightningUI_DurTime_Set;
                lightningItemGet = true;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(StarFallOn));
                break;
            //星落としパーティクルに当たった場合エフェクトを出しプレイヤーの速度を止め、カウンターを減らし、カウンターが0になったか確認する
            case "StarFallPS":
                localPlayer.SetRunSpeed(iceDurRunSpeed);
                localPlayer.SetWalkSpeed(iceDurWalkSpeed);
                localPlayer.SetStrafeSpeed(iceDurStrafeSpeed);
                localPlayer.SetJumpImpulse(iceDurJumpImpulse);
                localPlayer.SetGravityStrength(iceDurGravityStrength);
                vel = 0.0f * localPlayer.GetVelocity();
                localPlayer.SetVelocity(vel);
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(IceEffectOn));
                _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CL"));
                _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CC"));
                healShot.SendCustomEvent("GameSet");
                pillow_Item.SendCustomEvent("GameSet");
                break;
            //爆弾に当たった場合エフェクトを出しプレイヤーの速度を止め、カウンターを減らし、カウンターが0になったか確認する
            case "BombPS":
            case "FireCurve":
                localPlayer.SetRunSpeed(iceDurRunSpeed);
                localPlayer.SetWalkSpeed(iceDurWalkSpeed);
                localPlayer.SetStrafeSpeed(iceDurStrafeSpeed);
                localPlayer.SetJumpImpulse(iceDurJumpImpulse);
                localPlayer.SetGravityStrength(iceDurGravityStrength);
                vel = 0.0f * localPlayer.GetVelocity();
                localPlayer.SetVelocity(vel);
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(BombsEffectOn));
                _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CL"));
                _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CC"));
                healShot.SendCustomEvent("GameSet");
                pillow_Item.SendCustomEvent("GameSet");
                break;
            //銃弾に当たった場合エフェクトを出しHPを減らす
            case "BlasterPS":
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(BlasterEffectOn));
                break;
            //シールドアイテムに当たった場合持続効果時間を設定しエフェクトを出しコライダーを消す
            case "ShieldPS":
                if (localPlayer.GetRunSpeed() == 0.0f) return;
                shieldItem_DurTime = shieldItem_DurTime_Set;
                shieldItemGet = true;
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ShieldEffectOn));
                break;
            //ビックリマークエフェクト（闇）に当たった場合エフェクトを出しプレイヤーの速度を止め、カウンターを減らし、カウンターが0になったか確認する
            case "DarkPS":
                localPlayer.SetRunSpeed(iceDurRunSpeed);
                localPlayer.SetWalkSpeed(iceDurWalkSpeed);
                localPlayer.SetStrafeSpeed(iceDurStrafeSpeed);
                localPlayer.SetJumpImpulse(iceDurJumpImpulse);
                localPlayer.SetGravityStrength(iceDurGravityStrength);
                vel = 0.0f * localPlayer.GetVelocity();
                localPlayer.SetVelocity(vel);
                SendCustomNetworkEvent(NetworkEventTarget.All, nameof(DarkEffectOn));
                _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CL"));
                _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CC"));
                healShot.SendCustomEvent("GameSet");
                pillow_Item.SendCustomEvent("GameSet");
                break;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        //"user0"FindPlayerSphereColliderLayerName(layer == 22)
        //"user1"PlayerHitterColliderLayerName(layer == 23)
        //"user2"PlayerColliderLayerName(layer == 24)
        //"user3"NPCColliderLayerName(layer == 25)
        //"user5"JudgeCounterColliderLayerName(layer == 27)
        //"user6"SpeedUPItemColliderLayerName(layer == 28)
        //"user8"StarItemColliderLayerName(layer == 30)
        if(!Networking.IsOwner(gameObject)) return;
        //NPCに当たった場合エフェクトを出しプレイヤーの速度を止め、カウンターを減らし、カウンターが0になったか確認する
        if(other.gameObject.layer == 25) {
            localPlayer.SetRunSpeed(iceDurRunSpeed);
            localPlayer.SetWalkSpeed(iceDurWalkSpeed);
            localPlayer.SetStrafeSpeed(iceDurStrafeSpeed);
            localPlayer.SetJumpImpulse(iceDurJumpImpulse);
            localPlayer.SetGravityStrength(iceDurGravityStrength);
            Vector3 vel = 0.0f * localPlayer.GetVelocity();
            localPlayer.SetVelocity(vel);
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(IceEffectOn));
            _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CL"));
            _JudgeCounter.SendCustomNetworkEvent(NetworkEventTarget.Owner, ("CC"));
            healShot.SendCustomEvent("GameSet");
            pillow_Item.SendCustomEvent("GameSet");
        }
        //リスポーン時のコライダーに当たった場合コライダーを無効化し速度を元に戻す
        if (other.gameObject.layer == 27) {
            SetDefSpeed();
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(EnabledFalse));
        }
    }
    public void EnabledFalse()
    {
        p2N_SphereCol.enabled = false;
        p2N_SphereMesh.enabled = false;
        p2N_SphereCol2.enabled = false;
        p2N_SphereMesh2.enabled = false;
        p2P_SphereCol.enabled = false;
        p2P_SphereMesh.enabled = false;
    }
    public void SetDefSpeed() 
    {
        localPlayer.SetRunSpeed(playerdefRunSpeed);
        localPlayer.SetWalkSpeed(playerdefWalkSpeed);
        localPlayer.SetStrafeSpeed(playerdefStrafeSpeed);
        localPlayer.SetJumpImpulse(playerdefJumpImpulse);
        localPlayer.SetGravityStrength(playerdefGravityStrength);
    }
    public void IceEffectOn()
    {
        p2N_SphereCol.enabled = false;
        p2N_SphereMesh.enabled = false;
        p2N_SphereCol2.enabled = false;
        p2N_SphereMesh2.enabled = false;
        p2P_SphereCol.enabled = true;
        p2P_SphereMesh.enabled = true;
        iceParticle.SetActive(true);
        p2N_IceSound.Play();
    }
    public void SkullEffectOn()
    {
        p2N_SphereCol.enabled = false;
        p2N_SphereMesh.enabled = false;
        p2N_SphereCol2.enabled = false;
        p2N_SphereMesh2.enabled = false;
        p2P_SphereCol.enabled = true;
        p2P_SphereMesh.enabled = true;
        skullParticle.Play();
        iceParticle.SetActive(true);
        p2N_IceSound.Play();
    }
    public void BombsEffectOn()
    {
        p2N_SphereCol.enabled = false;
        p2N_SphereMesh.enabled = false;
        p2N_SphereCol2.enabled = false;
        p2N_SphereMesh2.enabled = false;
        p2P_SphereCol.enabled = true;
        p2P_SphereMesh.enabled = true;
        bombsParticle.Play();
        bombsSound.Play();
        iceParticle.SetActive(true);
        p2N_IceSound.Play();
    }
    public void DarkEffectOn()
    {
        p2N_SphereCol.enabled = false;
        p2N_SphereMesh.enabled = false;
        p2N_SphereCol2.enabled = false;
        p2N_SphereMesh2.enabled = false;
        p2P_SphereCol.enabled = true;
        p2P_SphereMesh.enabled = true;
        //darkParticle.Play();
        //darkSound.Play();
        iceParticle.SetActive(true);
        p2N_IceSound.Play();
    }
    public void LaserEffectOn()
    {
        laserSound.Play();
        laserParticle.SetActive(true);
    }
    public void LaserEffectOff()
    {
        laserParticle.SetActive(false);
    }
    public void SpeedUpEffectOn()
    {
        speedUpParticle.SetActive(true);
    }
    public void SpeedUpEffectOff()
    {
        speedUpParticle.SetActive(false);
    }
    public void StarEffectOn()
    {
        p2N_SphereCol.enabled = false;
        p2N_SphereMesh.enabled = false;
        p2N_SphereCol2.enabled = false;
        p2N_SphereMesh2.enabled = false;
        starParticle.SetActive(true);
    }
    public void StarEffectOff()
    {
        p2N_SphereCol.enabled = true;
        p2N_SphereMesh.enabled = true;
        p2N_SphereCol2.enabled = true;
        p2N_SphereMesh2.enabled = true;
        starParticle.SetActive(false);
    }
    public void PillowEffectOn()
    {
        pillowParticle.SetActive(true);
    }
    public void PillowEffectOff()
    {
        pillowParticle.SetActive(false);
    }
    public void BulletItemOn()
    {
        //このオブジェクトのオーナーの場合
        if(Networking.IsOwner(gameObject)) {
            bulletItem.transform.position = bulletItemRespawnPoint.position;
            bulletItem_Col.enabled = true;
        }
        bulletItem_Mesh.enabled = true;
        bullet_Particle.SetActive(true);
    }
    public void HealItemOn() {
        //このオブジェクトのオーナーの場合
        if(Networking.IsOwner(gameObject)) {
            healItem.transform.position = healItemRespawnPoint.position;
            healItem_Col.enabled = true;
        }
        heal_Particle.SetActive(true);
    }
    public void LightningUIOff() {
        lightning_WarningUI.SetActive(false);
    }
    public void StarFallOn() {
        if(!playerPlayingChecker.activeSelf) return;
        lightning_WarningUI.SetActive(true);
        starfallParticle.Play();
        starfallSound.Play();
    }
    public void BlasterEffectOn() {
        blasterHitParticle.Play();
    }
    public void ShieldEffectOn() {
        p2N_SphereCol.enabled = false;
        p2N_SphereMesh.enabled = false;
        p2N_SphereCol2.enabled = false;
        p2N_SphereMesh2.enabled = false;
        shieldParticle.SetActive(true);
    }
    public void ShieldEffectOff() {
        p2N_SphereCol.enabled = true;
        p2N_SphereMesh.enabled = true;
        p2N_SphereCol2.enabled = true;
        p2N_SphereMesh2.enabled = true;
        shieldParticle.SetActive(false);
    }
}
