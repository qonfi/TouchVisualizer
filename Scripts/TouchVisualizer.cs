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

        private DrawPositionCalculator positionCalculator;
        private TextDrawer textDrawer;


        private void Start()
        {
            positionCalculator = new DrawPositionCalculator(canvas);
            textDrawer = new TextDrawer(textPrefab);
        }


        private void Update()
        {
            textDrawer.SupplyText(this.gameObject);

            for (int i = 0; i<Input.touches.Length; i++)
            {
                // 移動を実行
                Vector2 drawPosition =  positionCalculator.Calc(Input.touches[i].position);
                textDrawer.MoveText(i, drawPosition);

                textDrawer.UpdateText(i);
            }
        }
    }
}