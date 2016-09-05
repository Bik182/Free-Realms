using log4net;

using SOE.Core;

// Global managers
using FreeRealms.Accounts;

// Gateway-specific managers
using Gateway.ClientLog;
using Gateway.Commands;
using Gateway.Login;

namespace Gateway
{
    public class Gateway : IRole
    {
        public static SOEServer Server;
        private static ILog Log;

        public void StartRole(SOEServer server)
        {
            // Set our server
            Server = server;

            // Get a logger
            Log = Server.Logger.GetLogger("Gateway");
            Log.Info("Constructing gateway...");

            // Construct our global managers
            AccountManager.Start(server);

            // Construct our managers
            CommandManager.Start();
            ClientLogManager.Start();
            LoginManager.Start();

            // Finish!
            Log.Info("Finished constructing gateway!");
        }
    }
}
