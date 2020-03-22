using Moq;
using Nuclear.Channels.Authentication.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Nuclear.Channels.UnitTests
{
    public class HttpListenerIdentityServiceTest
    {
        Mock<Func<string, string, bool>> mockBasicDelegate;
        Mock<Func<string, bool>> mockTokenDelegate;
        private HttpListenerIdentityService _identityService;

        public HttpListenerIdentityServiceTest()
        {
            mockBasicDelegate = new Mock<Func<string, string, bool>>();
            mockTokenDelegate = new Mock<Func<string, bool>>();
            _identityService = new HttpListenerIdentityService(mockBasicDelegate.Object, mockTokenDelegate.Object);
        }


        public static TheoryData<HttpListenerBasicIdentity> BasicIdentity
        {
            get
            {
                HttpListenerBasicIdentity basicIdentity = new HttpListenerBasicIdentity("username", "password");
                TheoryData<HttpListenerBasicIdentity> basic = new TheoryData<HttpListenerBasicIdentity>();
                basic.Add(basicIdentity);

                return basic;
            }
        }

        public static TheoryData<HttpListenerTokenIdentity> TokenIdentity
        {
            get
            {
                HttpListenerTokenIdentity tokenIdentity = new HttpListenerTokenIdentity("token");
                TheoryData<HttpListenerTokenIdentity> token = new TheoryData<HttpListenerTokenIdentity>();
                token.Add(tokenIdentity);

                return token;
            }
        }

        [Theory]
        [MemberData(nameof(BasicIdentity))]
        public void GetCredentialsForBasicIdentity_ReturnsUsernameAndPassword(HttpListenerBasicIdentity basic)
        {
            KeyValuePair<string, string> credentials = _identityService.GetCredentialsForBasicAuthentication(basic);

            Assert.True(!string.IsNullOrWhiteSpace(credentials.Key) && !string.IsNullOrWhiteSpace(credentials.Value));
            Assert.Equal("username", credentials.Key);
            Assert.Equal("password", credentials.Value);

        }

        [Theory]
        [MemberData(nameof(TokenIdentity))]
        public void GetCredentialsForTokenIdentity_ReturnsToken(HttpListenerTokenIdentity token)
        {
            string rToken = _identityService.GetCredentialsForTokenAuthentication(token);

            Assert.True(!string.IsNullOrWhiteSpace(rToken));
            Assert.Equal("token", rToken);
        }

    }
}
