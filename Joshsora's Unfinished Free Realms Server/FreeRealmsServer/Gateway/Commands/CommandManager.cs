using System.Collections.Generic;
using log4net;

using SOE;
using SOE.Core;
using SOE.Interfaces;

using FreeRealms.Accounts;

using Gateway.ClientLog;

namespace Gateway.Commands
{
    public static class CommandManager
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
            Log = Server.Logger.GetLogger("CommandManager");
            Log.Info("Service constructed");
        }

        [SOEMessageHandler((ushort)GatewayMessages.EnqueueCommand, "EnqueueCommand", "CGAPI_527")]
        public static void HandleEnqueueCommand(SOEClient sender, SOEMessage message)
        {
            // Is this client even authed?
            if (AccountManager.GetAccount(sender) == null)
            {
                Log.ErrorFormat("Unauthorized client tried enqueing command!");
                sender.Disconnect((ushort)SOEDisconnectReasons.Terminated);
                return;
            }

            // Setup a reader
            SOEReader reader = new SOEReader(message);

            // Get the length
            bool hasLength = reader.ReadBoolean(); // assuming this even is "has length" or just a random 01
            byte[] command = hasLength ? reader.ReadBlob() : reader.ReadToEnd();

            // Handle command!
            HandleCommand(sender, command);
        }

        public static void HandleCommand(SOEClient sender, byte[] command)
        {
            // Setup a reader
            SOEReader reader = new SOEReader(command);

            // Get the command code
            ushort commandOpCode = reader.ReadHostUInt16();

            // Handle
            switch (commandOpCode)
            {
                case ((ushort)ClientLogCommands.Log):
                    ClientLogManager.ReceiveLog(sender, command);
                    break;

                default:
                    Log.DebugFormat("Received unknown command! {0:X2}", commandOpCode);
                    break;
            }
        }
    }
}
