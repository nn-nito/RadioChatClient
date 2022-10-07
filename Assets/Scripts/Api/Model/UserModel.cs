using System;
using System.Collections;
using System.Collections.Generic;

namespace Api
{
    public class UserModel
    {
        [Serializable]
        public class User
        {
            public int id;
            public string name;
            public string user_code;
            public string registered_time;
            public string authentication_code;
        }

        [Serializable]
        public class Users
        {
            public User[] users;
        }
    }
}
