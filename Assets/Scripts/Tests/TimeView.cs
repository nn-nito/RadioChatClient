using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TimeView : MonoBehaviour
{
    [SerializeField] private TimeCounter timeCounter;
    [SerializeField] private Text counterText;
    // Start is called before the first frame update
    void Start()
    {
        timeCounter.OnTimeChanged.Subscribe(time => {
            // 現在のタイマ値をUIに反映する
            counterText.text = time.ToString();
        });
    }
}
