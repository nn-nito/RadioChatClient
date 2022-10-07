using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Chat
{
    public class ChatProvider : SingletonMonoBehaviour<ChatProvider>
    {
        public Button chatButton;
        public TMP_InputField chatInputField;
    }
}
