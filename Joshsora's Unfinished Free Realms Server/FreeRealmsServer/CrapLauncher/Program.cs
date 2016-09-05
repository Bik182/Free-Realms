using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;

namespace CrapLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter IP:");
            string ip = Console.ReadLine();

            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            // Get variables
            string loginServer = "http://freerealmsreconnected.com/api/";
            string apiRequest = loginServer + "login/";
            JObject response = null;

            using (var client = new WebClient { Proxy = null })
            {
                var values = new NameValueCollection();
                values["username"] = username;
                values["password"] = password;
                values["char_index"] = "0";

                var responseRaw = client.UploadValues(apiRequest, values);
                var responseString = Encoding.ASCII.GetString(responseRaw);
                try
                {
                    response = JObject.Parse(responseString);
                }
                catch (Exception)
                {
                    Console.WriteLine("Received bad response from login server!");
                    Environment.Exit(0);
                }
            }

            string token = response["token"].ToObject<string>();
            uint guid = response["guid"].ToObject<uint>();

            ProcessStartInfo process = new ProcessStartInfo("FreeRealms.exe");
            process.Arguments =
                string.Format(
                    "inifile=ClientConfig.ini Guid={0} Server={1}:20260 Ticket={2} ShowMemberLoadingScreen=1 key=Sm9zaHNvcmE=",
                    guid, ip, token
                );
            Process.Start(process);
        }
    }
}
