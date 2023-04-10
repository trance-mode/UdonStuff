
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LineRendererLaser : UdonSharpBehaviour
{
    public Transform startPoint;      // 線の始点となるゲームオブジェクト
    public Transform endPoint;        // 線の終点となるゲームオブジェクト

    private LineRenderer lineRenderer;    // Line Rendererコンポーネント

    void Start()
    {
        // Line Rendererコンポーネントを取得
        lineRenderer = GetComponent<LineRenderer>();
        // 線の始点と終点の座標を設定
        lineRenderer.SetPosition(0, startPoint.position);     // 始点の座標を設定
        lineRenderer.SetPosition(1, endPoint.position);       // 終点の座標を設定
    }
}
