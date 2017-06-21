//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // (needs NetworkBehaviour)
//using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;


namespace TouchVisualizer
{
    public class TextDrawer 
    {
        private Text textPrefab;
        private List<Text> textObjects;
        private List<RectTransform> textRects;


        public TextDrawer(Text _textPrefab)
        {
            this.textPrefab = _textPrefab;

            textObjects = new List<Text>();
            textRects = new List<RectTransform>();
        }


        public void SupplyText(GameObject visualizer)
        {
            // Text オブジェクトがタッチ数を下回って足りなくなったりしないように。
            // 変な書き方をしているかも。while 使うのほぼ初めて...
            while (Input.touches.Length > textObjects.Count)
            {
                GameObject baby =
                    GameObject.Instantiate(textPrefab.gameObject) as GameObject;    // ダウンキャスト
                Text babyText = baby.GetComponent<Text>();
                babyText.transform.SetParent(visualizer.transform, false);
                textObjects.Add(babyText);
                textRects.Add(babyText.GetComponent<RectTransform>());
            }
        }


        public void UpdateText(int index)
        {
            // 表示する文字列
            string s =
                "ID:" + Input.touches[index].fingerId + "\n" +
                "At:" + Input.touches[index].position + "\n" +
                "Phase:" + Input.touches[index].phase + "\n";
            textObjects[index].text = s;
        }


        public void MoveText(int index, Vector2 newPosition)
        {
            textRects[index].anchoredPosition = newPosition;
        }
    }
}