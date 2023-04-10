
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WallVelocityController : UdonSharpBehaviour
{
    [SerializeField] private float durationRunSpeed = 0.1f;
    [SerializeField] private float durationWalkSpeed = 0.1f;
    [SerializeField] private float durationStrafeSpeed = 0.1f;
    [SerializeField] private float durationJumpImpulse = 0.1f;
    [SerializeField] private float durationGravityStrength = 10.0f;
    [SerializeField] private float runSpeed = 4.0f;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float strafeSpeed = 2.0f;
    [SerializeField] private float jumpImpulse = 3.0f;
    [SerializeField] private float gravityStrength = 1.0f;
    [SerializeField] private AudioSource audioSource;
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        audioSource.Play();
        if (player == Networking.LocalPlayer) //自分が入った
        {
            if(player.GetRunSpeed() == 0.0f)
                return;
            player.SetRunSpeed(durationRunSpeed);
            player.SetWalkSpeed(durationWalkSpeed);
            player.SetStrafeSpeed(durationStrafeSpeed);
            player.SetJumpImpulse(durationJumpImpulse);
            player.SetGravityStrength(durationGravityStrength);
        }
        //npc.SendCustomEvent(Method); 
        //transform.position = transform.position + new Vector3(0, 500f, 0);   
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        audioSource.Stop();
        if (player == Networking.LocalPlayer) //自分が入った
        {
            if(player.GetRunSpeed() == 0.0f)
                return;
            player.SetRunSpeed(runSpeed);
            player.SetWalkSpeed(walkSpeed);
            player.SetStrafeSpeed(strafeSpeed);
            player.SetJumpImpulse(jumpImpulse);
            player.SetGravityStrength(gravityStrength);
        }
    }
}
