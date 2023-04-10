
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class layertest : UdonSharpBehaviour
{
    [SerializeField] int a;
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.layer == a){
            Debug.Log("24 In");
        }
        Debug.Log("Player In");
    }
}
