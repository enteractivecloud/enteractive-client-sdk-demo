using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Enteractive.SDK.Demo
{
    class Program
    {
        static async Task Main()
        {
            // Get credentials from configuration file 
            var configuration = Configure();
            string username = configuration.GetValue<string>("EnteractiveSDK:username");
            string password = configuration.GetValue<string>("EnteractiveSDK:password");

            //Initilize Enterective SDK using username and password
            EnteractiveClient.Init(username, password, Configuration.ConfigEnvironment.Staging);

            // Player Import
            PlayerImport playerImport = new PlayerImport();
            await playerImport.ImportPlayers();

            // Player Synchronization
            PlayerUpdate playerSynchronization = new PlayerUpdate();
            await playerSynchronization.UpdatePlayers();
        }

   
        /// <summary>
        /// Configuration builder
        /// </summary>
        /// <returns></returns>
        public static IConfiguration Configure()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

    }
}
