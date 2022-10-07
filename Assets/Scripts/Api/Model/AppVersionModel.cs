using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Api.Model
{
    public class AppVersionModel
    {
        [Serializable]
        public class AppVersion
        {
            public string version;
            public int platform_id;
            public string url;
            public string title;
            public string message;
        }

        [Serializable]
        public class AppVersions
        {
            public AppVersion[] appVersions;
        }
    }
}
