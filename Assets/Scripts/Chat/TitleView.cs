using Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    // Start is called before the first frame update
    void Start()
    {
        _titleText.text = Boot.Instance.RadioTitle;
    }
}
