
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections;
using System.Collections.Generic;

public class HeadCollider : UdonSharpBehaviour
{
    public GameObject character;
    public GameObject HandsIn_AudioSource;
    public GameObject Eat_AudioSource;
    public GameObject Target;
    public GameObject particleObject;
    private Animator animator;
    private AudioSource HandsIn_SE;
    private AudioSource Eat_SE;
    private ParticleSystem Particle;

    [SerializeField] private VRC_Pickup _vrcPickup;
    
    [UdonSynced(UdonSyncMode.None)]
    private bool stay = false;
    private float time = 0.0f;  
    private bool isRespon = false;
    [SerializeField] private int IfHandsEnter;
    [SerializeField] private int IfHandsExit;
    [SerializeField] private int IfEatEnter;
    [SerializeField] private int IfEatExit;
    [SerializeField] private Vector3 Scale;
    [SerializeField] private Vector3 MaxScale;
    
    public void Start()
    {
        var player = Networking.LocalPlayer;
        Networking.SetOwner(player, character.gameObject);
        animator = character.GetComponent<Animator>();      
        HandsIn_SE = HandsIn_AudioSource.GetComponent<AudioSource>();
        Eat_SE = Eat_AudioSource.GetComponent<AudioSource>();
        Particle = particleObject.GetComponent<ParticleSystem>();
    }
    public void Update()
    {
        if (stay == true && Target.transform.localScale.x >= 0.02f)
        {
            isRespon = true;
            Target.transform.localScale += new Vector3(Scale.x, Scale.y, Scale.z);
        }
        else if (Target.transform.localScale.x < 0.02f)
        {
            isRespown();
        }
    }
    public void isRespown()
    {
        if(isRespon){
            _vrcPickup.Drop();
            Target.transform.localScale = new Vector3(0, 0, 0);
            animator.SetTrigger("Exit Shape");
            animator.SetInteger("Shape Int", IfEatExit);
            Debug.Log("EatExit");
            Eat_SE.Stop();
            Particle.Stop();
            Debug.Log("isRespon");
            time += Time.deltaTime;
            if(time >= 3.0f){
                Debug.Log("time >= 3.0f");
                isRespon = false;
                Vector3 pos = Target.transform.position;
                pos.y = -200.0f;
                Target.transform.position = pos;
                Target.transform.localScale = new Vector3(MaxScale.x, MaxScale.y, MaxScale.z);
                time=0.0f;
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if( layerName == "user6")   //PlayerInteractionCollider
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HandsTriggerEnter"); 
        }
        else if( layerName == "user7")  //IfEatCollider
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EatTriggerEnter");
        }
    }

    public void OnTriggerStay(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "user7")   //IfEatCollider
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EatTriggerStay");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if( layerName == "user6")   //PlayerInteractionCollider
        {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HandsTriggerExit");
        }
        else if( layerName == "user7")  //IfEatCollider
        {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EatTriggerExit");
        }
    }
    public void HandsTriggerEnter()
    {
            animator.SetTrigger("Exit Shape");
            animator.SetInteger("Shape Int", IfHandsEnter);
            Debug.Log("HandsEnter");
            HandsIn_SE.Play();   
    }
    public void HandsTriggerExit()
    {
            animator.SetTrigger("Exit Shape");
            animator.SetInteger("Shape Int", IfHandsExit);
            Debug.Log("HandsExit");
            HandsIn_SE.Stop();  
    }
        public void EatTriggerEnter()
    {
            animator.SetTrigger("Exit Shape");
            animator.SetInteger("Shape Int", IfEatEnter);
            Debug.Log("EatEnter");
            Eat_SE.Play();
            Particle.Play();
    }
    public void EatTriggerStay()
    {
            stay = true;
    }

    public void EatTriggerExit()
    {
            animator.SetTrigger("Exit Shape");
            animator.SetInteger("Shape Int", IfEatExit);
            Debug.Log("EatExit");
            Eat_SE.Stop();
            Particle.Stop();
            stay = false;
    }
}
