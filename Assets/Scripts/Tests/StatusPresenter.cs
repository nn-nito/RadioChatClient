using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StatusPresenter : MonoBehaviour
{
    public StatusModel model;

    public ParameterBarView healthView;

    private void Awake() {
        model.healthRP.Subscribe(value => { healthView.SetRate(model.healthMax, value); });
    }
}
