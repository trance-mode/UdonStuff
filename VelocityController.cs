
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
public class VelocityController : UdonSharpBehaviour{
    // public NPCController4 npc; 
    // public string Method;   
    // public string Method2; 
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
