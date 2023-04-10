
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class RespawnTeleportCollider : UdonSharpBehaviour 
{
    [Space(20)]
	[Header("-----------------------------プレイヤーデフォルトスピード設定-----------------------------")]
	[Space(20)]
    [SerializeField] float playerRunSpeed = 4.0f;
    [SerializeField] float playerWalkSpeed = 2.0f;
    [SerializeField] float playerStrafeSpeed = 2.0f;
    [SerializeField] float playerJumpImpulse = 3.0f;
    [SerializeField] float playerGravityStrength = 1.0f;
    
    [Space(20)]
	[Header("-----------------------------テレポートコライダーを表示する時間-----------------------------")]
	[Space(20)]
    [SerializeField] float totalTime;

    [Space(20)]
	[Header("-----------------------------テレポートする場所-----------------------------")]
	[Space(20)]
    [SerializeField] Transform targetSpawn;

    [Space(20)]
	[Header("-----------------------------カウンター設定-----------------------------")]
	[Space(20)]
    public JudgeCounter _JudgeCounter;

    [Space(20)]
	[Header("-----------------------------プレイヤーがゲームプレイしているかどうか判断するフラグ-----------------------------")]
	[Space(20)]
    [SerializeField] GameObject playerPlayingChecker;
    //表示されたら表示時間を1秒に設定する
    void OnEnable() 
    {
		totalTime = 1;
	}

    //一秒経ったらこのオブジェクトを非表示にする
    void Update() 
    {
        totalTime -= Time.deltaTime;
        if (totalTime <= 0) {
            totalTime = 1;
            this.gameObject.SetActive(false);
        }
    }
    //プレイヤーがこのコライダーに一回触れた時、プレイヤーのスピードをデフォルトに戻し、プレイヤーを最初の場所にテレポートさせる
    public override void OnPlayerTriggerStay(VRCPlayerApi player) 
    {   
        //自分自身が触れている場合のみ処理を行う
        if (player == Networking.LocalPlayer) {
            //プレイヤーがゲームプレイをやめたことにする
            playerPlayingChecker.SetActive(false);
            //デフォルトに戻す
            player.SetRunSpeed(playerRunSpeed);
            player.SetWalkSpeed(playerWalkSpeed);
            player.SetStrafeSpeed(playerStrafeSpeed);
            player.SetJumpImpulse(playerJumpImpulse);
            player.SetGravityStrength(playerGravityStrength);
            //プレイヤーを最初の場所にテレポートさせる
            Networking.LocalPlayer.TeleportTo(targetSpawn.position, targetSpawn.rotation);
            //一回だけの処理でよいので、このオブジェクトを非表示にする
            this.gameObject.SetActive(false);
        }
    }
}
