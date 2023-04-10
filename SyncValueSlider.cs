using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

public class SyncValueSlider : UdonSharpBehaviour
{
    [UdonSynced(UdonSyncMode.Linear)] float someValue = 0;
    public Slider valueSlider;
    [SerializeField] TextMeshProUGUI targetText = null;
    void Start(){
        GetSliderValue();
        targetText.text = someValue.ToString();
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
        someValue = valueSlider.value;
    }
}
