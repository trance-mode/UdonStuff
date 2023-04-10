
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RandomSpawn : UdonSharpBehaviour
{
    // 円の半径
    [SerializeField] private float _radius = 1;
    // 円の中心点
    [SerializeField] private Vector3 _center;
    // 配置するPrefab
    public GameObject[] spawnObjects;
    void Update()
    {
        for(int i = 0; i < spawnObjects.Length; i++)
        {
            if (!spawnObjects[i].activeSelf) break;
            // 指定された半径の円内のランダム位置を取得
            var circlePos = _radius * Random.insideUnitCircle;

            // XZ平面で指定された半径、中心点の円内のランダム位置を計算
            var spawnPos = new Vector3(
                circlePos.x, 0, circlePos.y
            ) + _center;
 
            spawnObjects[i].transform.position = spawnPos;
        }    
    }
}
