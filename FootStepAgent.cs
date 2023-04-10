
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.AI;

namespace FootStepSystem {
    public class FootStepAgent : UdonSharpBehaviour {

        NavMeshAgent agent;
        [SerializeField] AudioSource footStepAudioSource;
        [SerializeField] float speedThreshold = 3f;
        [SerializeField] float speedCoefficient = 0.15f;
        [SerializeField] float defaultPitch = 1;
        [SerializeField] AudioClip footStepSound;
        bool previousGrounded = true;
        bool presentGrounded = true;
        float agentSpeed = 0;
        void Start() {
            footStepAudioSource.clip = footStepSound;
            agent = GetComponent<NavMeshAgent>();
        }
        void Update() {
            presentGrounded = true;

            // Agentの速度情報を取得
            agentSpeed = agent.velocity.magnitude;

            // Agentが接地しているかどうかを確認
            if (agentSpeed > speedThreshold) {
                footStepAudioSource.mute = false;
                footStepAudioSource.pitch = (float)(agentSpeed * speedCoefficient + defaultPitch);
                footStepAudioSource.transform.position = transform.position;
            }
            else {
                footStepAudioSource.mute = true;
            }
            previousGrounded = presentGrounded;
        }
    }
}