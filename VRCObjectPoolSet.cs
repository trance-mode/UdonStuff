using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class VRCObjectPoolSet : UdonSharpBehaviour
{
    [SerializeField] private VRCObjectPool _vrcObjectPool1;
    private GameObject[] _pools1;
    private void Start()
    {
        // オブジェクトプール分の配列を確保
        _pools1 = new GameObject[_vrcObjectPool1.Pool.Length];
    }
    public void IceParticleSpawn()
    {
        VRCPlayerApi targetPlayer = Networking.GetOwner(this.gameObject);
        _pools1[0] = _vrcObjectPool1.TryToSpawn();
        _pools1[0].transform.position = targetPlayer.GetPosition();
    }
    public void IceParticleReturn()
    {
        _vrcObjectPool1.Return(_pools1[0]);
    }
}