using SOE.Core;

namespace Gateway.Login
{
    public struct LoginRequest
    {
        public SOEClient Client;
        public string Token;
        public string Version;

        public LoginRequest(SOEClient client, string token, string version)
        {
            Client = client;
            Token = token;
            Version = version;
        }
    }
}
