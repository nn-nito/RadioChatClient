using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UniRxTest : MonoBehaviour
{
    Subject<string> subject = new Subject<string>();
    // Start is called before the first frame update
    void Start()
    {
        subject.Subscribe(msg => Debug.Log("Subscribe1:" + msg));
        subject.Subscribe(msg => Debug.Log("Subscribe2:" + msg));
        subject.Subscribe(msg => Debug.Log("Subscribe3:" + msg));

        subject.OnNext("こんにちは");
        subject.OnNext("おはよう");
    }
}
