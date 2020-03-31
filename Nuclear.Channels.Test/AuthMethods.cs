using System.Collections.Generic;
using System.Security.Claims;

namespace Nuclear.Channels.Test
{
    public class AuthMethods
    {
        public bool AuthenticateBasic(string username, string password)
        {
            return true;
        }

        //public bool AuthenticateToken(string token)
        //{
        //    return true;
        //}

        public Claim[] AuthenticateToken(string token)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Role","Admin")
            };

            return claims.ToArray();
        }
    }
}
