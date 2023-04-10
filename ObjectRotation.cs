
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ObjectRotation : UdonSharpBehaviour
{
    [SerializeField] private float RotateSpeed = 2.0f;
    [SerializeField] private Quaternion _initialRotation; // 初期回転
    void Awake()
    {
        _initialRotation = gameObject.transform.rotation; 
    }
    void OnEnable() {
        gameObject.transform.rotation = _initialRotation; // 回転の初期化
    }
    void Update()
    {
        transform.Rotate(RotateSpeed * Time.deltaTime * Vector3.right);
    }
}
