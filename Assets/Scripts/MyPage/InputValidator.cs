using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MyPage
{
    public class InputValidator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _errorText;
        public string _errorNumberMessage = "文字数は1～10文字以内でお願いいたします";
        public string _errorExcludeNumberMessage = "含めることのできない文字が存在いいたします";

        public bool Validate(string text)
        {
            if (text.Length > 10 || text.Length < 1)
            {
                _errorText.gameObject.SetActive(true);
                _errorText.text = _errorNumberMessage;

                return false;
            }

            if (text.Contains("<br>") || text.Contains("\\n"))
            {
                _errorText.gameObject.SetActive(true);
                _errorText.text = _errorExcludeNumberMessage;

                return false;
            }

            _errorText.gameObject.SetActive(false);
            return true;
        }
    }
}
