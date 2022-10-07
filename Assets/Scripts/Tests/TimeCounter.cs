using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimeCounter : MonoBehaviour
{
    // イベントを発行する核となるインスタンス
    private Subject<int> timerSubject = new Subject<int>();

    // イベントの購読側だけを公開
    public IObservable<int> OnTimeChanged => timerSubject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    // Update is called once per frame
    IEnumerator TimerCoroutine()
    {
        var time = 100;
        while (time > 0) {
            time--;
            // イベントを発行
            timerSubject.OnNext(time);
            // 一秒待つ
            yield return new WaitForSeconds(1);
        }
    }
}
