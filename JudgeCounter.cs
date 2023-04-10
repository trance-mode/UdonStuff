
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class JudgeCounter : UdonSharpBehaviour
{
    [Space(20)]
    [Header("UdonSynced")]
	[Header("-----------------------------カウンター-----------------------------")]
	[Space(20)]
    [UdonSynced] public int intdata;
    [Space(20)]
	[Header("-----------------------------カウンター表示用テキスト-----------------------------")]
	[Space(20)]
    [SerializeField] TextMeshProUGUI intdataText;
    [SerializeField] TextMeshProUGUI timeUpText;
    [Space(20)]
	[Header("-----------------------------リスポーン時判定用コライダー-----------------------------")]
	[Space(20)]
    [SerializeField] BoxCollider judgeCollider;
    [SerializeField] MeshRenderer judgeMeshRenderer;
    [Space(20)]
	[Header("-----------------------------タイマーシステム-----------------------------")]
	[Space(20)]
    public Vket5.Circle2539.Timer.SyncTimerSystem _SyncTimerSystem;
    
    private void OnTriggerEnter(Collider other) 
    {
        if (!Networking.IsOwner(Networking.LocalPlayer, other.gameObject)) return;
        if (other.gameObject.layer == 24) {
            SendCustomNetworkEvent(NetworkEventTarget.Owner, nameof(CL));
            SendCustomNetworkEvent(NetworkEventTarget.Owner, nameof(CC));
        }
        RequestSerialization();
    }
    //CountAdd
    public void CA() 
    {
        intdata = intdata + 1;
        intdataText.text = "Survivor\n" + intdata.ToString();
        timeUpText.text = "Survivor\n" + intdata.ToString();
        RequestSerialization();
    }
    //CountLess
    public void CL() 
    {
        intdata = intdata - 1;
        intdataText.text = "Survivor\n" + intdata.ToString();
        timeUpText.text = "Survivor\n" + intdata.ToString();
        RequestSerialization();
    }
    //CountCheck
    public void CC() 
    {
        if(intdata <= 0) {
            _SyncTimerSystem.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EG");
        }
    }
    //ResetJudgeCounter
    public void RJC() 
    {
        intdata = 0;
        intdataText.text = "Survivor\n" + intdata.ToString();
        timeUpText.text = "Survivor\n" + intdata.ToString();
        RequestSerialization();
    }
    public override void OnDeserialization() 
    {
        intdataText.text = "Survivor\n" + intdata.ToString();
        timeUpText.text = "Survivor\n" + intdata.ToString();
    }
}
