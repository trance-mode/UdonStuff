
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Components;
using VRC.Udon;

public class ColliderEnterPlayer : UdonSharpBehaviour
{
    [SerializeField] float playerRunSpeed = 4.0f;
    [SerializeField] float playerWalkSpeed = 2.0f;
    [SerializeField] float playerStrafeSpeed = 2.0f;
    [SerializeField] float playerJumpImpulse = 3.0f;
    [SerializeField] float playerGravityStrength = 1.0f;
    public VRCObjectPoolSet iceParticlePoolSet;
    [HideInInspector] public VRCPlayerApi localPlayer;
    public VRCPlayerApi targetPlayer;
    void Start() 
    {
        localPlayer = Networking.LocalPlayer;
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == localPlayer) return;
        iceParticlePoolSet.SendCustomEvent("IceParticleReturn"); 
        targetPlayer = Networking.GetOwner(this.gameObject);
        targetPlayer.SetRunSpeed(playerRunSpeed);
        targetPlayer.SetWalkSpeed(playerWalkSpeed);
        targetPlayer.SetStrafeSpeed(playerStrafeSpeed);
        targetPlayer.SetJumpImpulse(playerJumpImpulse);
        targetPlayer.SetGravityStrength(playerGravityStrength);
    }
}
