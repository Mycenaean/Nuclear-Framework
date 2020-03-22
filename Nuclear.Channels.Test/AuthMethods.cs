namespace Nuclear.Channels.Test
{
    public class AuthMethods
    {
        public bool AuthenticateBasic(string username, string password)
        {
            return true;
        }

        public bool AuthenticateToken(string token)
        {
            return true;
        }
    }
}
