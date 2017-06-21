//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // (needs NetworkBehaviour)
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
using UnityEngine;
using System;


namespace TouchVisualizer
{
    public class DrawPositionCalculator 
    {
        private Canvas canvas;
        private RectTransform canvasRect;
        private Vector2 readableOffset;


        public DrawPositionCalculator(Canvas _canvas)
        {
            this.canvas = _canvas;
            canvasRect = canvas.GetComponent<RectTransform>();
            readableOffset = new Vector2(0.0f, 100.0f);
        }


        public Vector2 Calc(Vector2 touchPosition)
        {
            // touches.position はスクリーン座標(ピクセル)が帰ってくる。
            // これをビューポート座標(画面端から反対の端までが 0~1 の値)に変換
            Camera cam = Camera.main;
            Vector2 positionScaleOnScreen = cam.ScreenToViewportPoint(touchPosition);

            // コードをすっきりさせるために。しかし実際にキャンバスの幅や高さなのかは謎。
            float canvasWidth = canvasRect.sizeDelta.x;
            float canvasHeight = canvasRect.sizeDelta.y;

            // この辺は自分でもよくわからない。
            // 上で出したビューポート座標の値を、画面全体に対しての割合のように使った。
            Vector2 drawPosition = new Vector2(
                positionScaleOnScreen.x * canvasWidth,
                positionScaleOnScreen.y * canvasHeight
                );

            // 指で見えない場所を避けるため少しずらす。
            drawPosition = drawPosition + readableOffset;

            // 見づらいので画面の端っこまでは行かないように、値を制限する
            drawPosition = new Vector2(
                Mathf.Clamp(drawPosition.x, canvasWidth * 0.20f, canvasWidth * 0.80f),
                Mathf.Clamp(drawPosition.y, canvasHeight * 0.20f, canvasHeight * 0.80f)
                );

            return drawPosition;
        }
    }
}