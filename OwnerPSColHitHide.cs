
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using System.Collections;
using VRC.Udon;

[RequireComponent(typeof(ParticleSystem))]
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class OwnerPSColHitHide : UdonSharpBehaviour
{
    ParticleSystem ps;
    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.CollisionModule collision = ps.collision;
        collision.enabled = true;
        collision.type = ParticleSystemCollisionType.World;
        collision.mode = ParticleSystemCollisionMode.Collision3D;
        collision.lifetimeLoss = 0f;
    }
    //オーナーが撃ったオブジェクトは他のプレイヤーに当たり自分には当たらない、他のプレイヤー
    //other=プレイヤー
    //プレイヤーの
    public void OnParticleCollision(GameObject playerCollider)
    {
        if(playerCollider.gameObject.layer == 24) {
            //このオブジェクトのオーナーと当たったオブジェクトのオーナーが同じなら
            if(Networking.IsOwner(playerCollider.gameObject) == Networking.IsOwner(gameObject)) {
                return;
            } 
            else {
                //パーティクルのコライダーを取得
                ParticleSystem.CollisionModule collision = ps.collision;
                //パーティクルのコライダーを消す
                collision.lifetimeLoss = 1f;
            }
        }
        else{
            //パーティクルのコライダーを取得
            ParticleSystem.CollisionModule collision = ps.collision;
            //パーティクルのコライダーを消す
            collision.lifetimeLoss = 1f;
        }
    }
}
