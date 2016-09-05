using FreeRealms.Accounts;
using log4net;
using SOE.Core;
using SOE.Interfaces;

namespace Gateway.ClientLog
{
    public static class ClientLogManager
    {
        // Components
        private static SOEServer Server;
        private static ILog Log;

        public static void Start(SOEServer server = null)
        {
            // Set our server
            if (server == null)
            {
                Server = Gateway.Server;
            }
            else
            {
                Server = server;
            }

            // Logging
            Log = Server.Logger.GetLogger("ClientLogManager");
            Log.Info("Service constructed");
        }

        public static void ReceiveLog(SOEClient sender, byte[] command)
        {
            // Setup a reader
            SOEReader reader = new SOEReader(command);
            reader.ReadHostUInt16();

            // Get the manager and message
            string manager = reader.ReadASCIIString().Replace(".log", "");
            string message = reader.ReadASCIIString();

            // Get the logger for the manager
            ILog log = Server.Logger.GetLogger(manager);
            int clientId = sender.GetClientID();
            string username = AccountManager.GetAccount(sender).Username;

            // Is this a critical error?
            if (manager == "ClientCriticalError")
            {
                log.ErrorFormat("[{0}, {1}] {2}", clientId, username, message);
            }
            else
            {
                log.InfoFormat("[{0}, {1}] {2}", clientId, username, message);
            }
        }
    }
}
