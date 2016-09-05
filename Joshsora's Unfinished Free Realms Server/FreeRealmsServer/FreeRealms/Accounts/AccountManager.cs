using System.Collections.Generic;
using log4net;
using SOE;
using SOE.Core;

namespace FreeRealms.Accounts
{
    public static class AccountManager
    {
        // Components
        private static SOEServer Server;
        private static ILog Log;

        // Account maps
        private static Dictionary<SOEClient, Account> Client2Account;
        private static Dictionary<uint, SOEClient> ID2Client;
        private static Dictionary<uint, Account> ID2Account;
        private static Dictionary<string, Account> Username2Account;

        public static void Start(SOEServer server)
        {
            // Set our server
            Server = server;

            // Create empty maps
            Client2Account = new Dictionary<SOEClient, Account>();
            ID2Client = new Dictionary<uint, SOEClient>();
            ID2Account = new Dictionary<uint, Account>();
            Username2Account = new Dictionary<string, Account>();

            // Handle disconnect event
            Server.ConnectionManager.OnDisconnect += OnClientDisconnect;

            // Logging
            Log = Server.Logger.GetLogger("AccountManager");
            Log.Info("Service constructed");
        }

        public static void OnClientDisconnect(SOEClient client)
        {
            // Does this client have an account?
            if (Client2Account.ContainsKey(client))
            {
                // Log them out!
                Logout(GetAccount(client));
            }
        }

        public static void AddAccount(SOEClient owner, Account account)
        {

            // Are they already logged in?
            if (ID2Account.ContainsKey(account.ID))
            {
                // Uh oh! Log the old one out..
                Log.Warn("A user has logged into an account that is already in use! Disconnecting old client!");
                ID2Client[account.ID].Disconnect((ushort) SOEDisconnectReasons.NewConnection);
            }

            // Log
            Log.InfoFormat("Account '{0}' has successfully logged in!", account.Username);

            // Add the new one!
            ID2Client.Add(account.ID, owner);
            Client2Account.Add(owner, account);
            ID2Account.Add(account.ID, account);
            Username2Account.Add(account.Username, account);
        }
        
        public static void Logout(Account account)
        {
            // Log
            Log.InfoFormat("Logging out '{0}'!", account.Username);

            // Is this user logged in?
            if (ID2Account.ContainsKey(account.ID))
            {
                // Remove them from our maps..
                Client2Account.Remove(ID2Client[account.ID]);
                ID2Client.Remove(account.ID);
                ID2Account.Remove(account.ID);
                Username2Account.Remove(account.Username);
            }
        }

        public static Account GetAccount(SOEClient client)
        {
            if (Client2Account.ContainsKey(client))
            {
                return Client2Account[client];
            }

            return null;
        }


        public static Account GetAccount(uint id)
        {
            if (ID2Account.ContainsKey(id))
            {
                return ID2Account[id];
            }

            return null;
        }

        public static Account GetAccount(string username)
        {
            if (Username2Account.ContainsKey(username))
            {
                return Username2Account[username];
            }

            return null;
        }

        public static SOEClient GetClient(uint id)
        {
            if (ID2Client.ContainsKey(id))
            {
                return ID2Client[id];
            }

            return null;
        }

        public static bool IsAuthorized(SOEClient client)
        {
            return Client2Account.ContainsKey(client);
        }
    }
}
