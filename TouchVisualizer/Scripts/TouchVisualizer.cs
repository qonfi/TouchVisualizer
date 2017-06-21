using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;


namespace TouchVisualizer
{
    public class TouchVisualizer : MonoBehaviour
    {
        [SerializeField]  private Text textPrefab;
        [SerializeField] private Canvas canvas;
        private RectTransform canvasRect;
        List<Text> texts;         // タップされた座標に表示する UnityEngine.UI.Text 
        List<RectTransform> textRects;
        private Vector2 readableOffset;

        private void Start()
        {
            texts = new List<Text>();
            textRects = new List<RectTransform>();
            canvasRect = canvas.GetComponent<RectTransform>();
            readableOffset = new Vector2(0.00f, 100f);
        }


        private void Update()
        {
            SupplyText();

            for (int i = 0; i<Input.touches.Length; i++)
            {
                // touches.position はスクリーン座標(ピクセル)が帰ってくる。
                // これをビューポート座標(画面端から反対の端までが 0~1 の値)に変換
                Camera cam = Camera.main;
                Vector2 positionScaleOnScreen =  cam.ScreenToViewportPoint(Input.touches[i].position);
                
                // コードをすっきりさせるために。しかし実際にキャンバスの幅や高さなのかは謎。
                float canvasWidth = canvasRect.sizeDelta.x;
                float canvasHeight = canvasRect.sizeDelta.y;

                // この辺は自分でもよくわからない。
                // 上で出したビューポート座標の値を、画面全体に対しての割合のように使った。
                Vector2 drawPosition = new Vector2(
                    positionScaleOnScreen.x * canvasWidth,
                    positionScaleOnScreen.y * canvasHeight
                    );

                drawPosition = drawPosition + readableOffset;

                // 見づらいので画面の端っこまでは行かないように、値を制限する
                drawPosition = new Vector2(
                    Mathf.Clamp(drawPosition.x, canvasWidth * 0.20f, canvasWidth * 0.80f ),
                    Mathf.Clamp( drawPosition.y, canvasHeight * 0.20f , canvasHeight * 0.80f)
                    );

                // 移動を実行
                textRects[i].anchoredPosition = drawPosition;

                // 表示する文字列
                string s =
                    "ID:" + Input.touches[i].fingerId + "\n" +
                    "At:" + Input.touches[i].position + "\n" +
                    "Phase:" + Input.touches[i].phase + "\n";
                texts[i].text = s;
            }
        }

        


        private void SupplyText()
        {
            // Text オブジェクトがタッチ数を下回って足りなくなったりしないように。
            // 変な書き方をしているかも。while 使うのほぼ初めて...
            while (Input.touches.Length > texts.Count)
            {
                GameObject baby = 
                    GameObject.Instantiate(textPrefab.gameObject) as GameObject;    // ダウンキャスト
                Text babyText = baby.GetComponent<Text>();
                babyText.transform.SetParent(this.transform, false);
                texts.Add(babyText);
                textRects.Add(babyText.GetComponent<RectTransform>());
            }
        }
    }
}