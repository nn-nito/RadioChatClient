using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Api
{
    public class UserFavoriteRadioModel
    {
        [Serializable]
        public class UserFavoriteRadio
        {
            public int user_id;
            public int radio_id;
        }

        [Serializable]
        public class UserFavoriteRadios
        {
            public UserFavoriteRadio[] userFavoriteRadios;
        }
    }
}
