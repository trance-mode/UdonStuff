
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ShieldItem : UdonSharpBehaviour
{
    
    [SerializeField] private float durationTimeSet = 5f;
    [SerializeField] private float durationTime = 0f;
    [SerializeField] private GameObject item;
    [SerializeField] private ParticleSystem[] particleArray;
    [SerializeField] private bool itemGet = false;
    [SerializeField] private SphereCollider sphereCol;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip hitSound;
    void Start(){
        sphereCol = GetComponent<SphereCollider>();
    }
    void Update(){
        if (itemGet){
            durationTime -= Time.deltaTime;
            if (durationTime <= 0){
                itemGet = false;
                durationTime = durationTimeSet;
                sphereCol.enabled = true;
                item.SetActive(true);
                audioSource.PlayOneShot(spawnSound);
            }
        }
    }
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 24){  //user2
            //あたったobjのscriptのなかのshield_flagって言う変数をtrueに帰るやつ
            UdonBehaviour udonscript = (UdonBehaviour)other.GetComponent(typeof(UdonBehaviour));
            if(udonscript != null){//もしなかったらエーラ出ないようにかえるやつ
                udonscript.SendCustomEvent("shield_active");
            }
            durationTime = durationTimeSet;
            itemGet = true;
            audioSource.PlayOneShot(hitSound);
            sphereCol.enabled = false;
            item.SetActive(false);
            for (int count=0; count<particleArray.Length; count++){
                particleArray[count].Play();
            }
        } 
    }
}
