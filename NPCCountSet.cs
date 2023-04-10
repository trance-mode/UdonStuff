
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
using VRC.Udon.Common.Interfaces;

public class NPCCountSet : UdonSharpBehaviour
{
    [UdonSynced(UdonSyncMode.Linear)] int someValue = 0;
    public Slider valueSlider;
    [SerializeField] TextMeshProUGUI targetText = null;
    public GameObject[] nPC;
    void Start(){
        GetSliderValue();
        targetText.text = someValue.ToString();
        SerializeValue();
    }
    void LateUpdate()
    {
        valueSlider.value = someValue;
        targetText.text = someValue.ToString();
    }
    public void OnChangeOwner()
    {
        if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        }
    }
    public void GetSliderValue()
    {
        someValue = (int)valueSlider.value;
    }
    public void EndDrag()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SerializeValue));
    }
    public void SerializeValue()
    {
        //NPCを一旦全て非表示にする
        for(int i = 0; i < nPC.Length; i++)
        {
            nPC[i].SetActive(false);
        }
        //somevalueの数だけNPCを表示する
        for(int i = 0; i < someValue; i++)
        {
            nPC[i].SetActive(true);
        }
    }
}
