using System;
using System.Collections.Generic;
using System.Text;

namespace Nuclear.Channels.Test
{
    public class AuthMethods
    {
        public bool AuthenticateBasic(string username,string password)
        {
            return true;
        }

        public bool AuthenticateToken(string token)
        {
            return true;
        }
    }
}
