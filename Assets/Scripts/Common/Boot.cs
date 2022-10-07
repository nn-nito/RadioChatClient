using Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class Boot : SingletonMonoBehaviour<Boot>
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public int RoomId { get; set; }
        public string RadioTitle { get; set; }
        public string CurrentScene = SceneList.HOME;
        public string BeforeScene = SceneList.HOME;
        public int StartingCount { get; set; }
        public string BeforeStartingDay { get; set; }

        override protected void Awake()
        {
            Application.targetFrameRate = 40;
            //if (Application.platform == RuntimePlatform.Android)
            //{
            //    Input.backButtonLeavesApp = true;
            //}

            base.Awake();
            DontDestroyOnLoad(this);

            ZPlayerPrefs.Initialize("d87jbuiioau8376duu2", "dg88jihbguia8tyi");
            Boot.Instance.StartingCount = ZPlayerPrefs.GetInt("startingCount");
            Boot.Instance.BeforeStartingDay = ZPlayerPrefs.GetString("beforeStartingDay");
            Boot.Instance.UserId = ZPlayerPrefs.GetInt("userId");
            Boot.Instance.UserName = ZPlayerPrefs.GetString("userName");
            Boot.Instance.UserCode = ZPlayerPrefs.GetString("userCode");
        }
    }
}
