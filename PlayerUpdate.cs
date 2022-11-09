using Enteractive.SDK.Demo.Models;
using Enteractive.SDK.Extensions;
using Enteractive.SDK.Models.V2.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enteractive.SDK.Demo
{
    /// <summary>
    /// This class demonstrates the process of synchronizing players data
    /// </summary>
    class PlayerUpdate
    {
        private List<string> Brands;
        private readonly Dictionary<string, string> BrandNamesMap = new Dictionary<string, string>();

        /// <summary>
        /// Player Synchronization process
        /// </summary>
        /// <returns></returns>
        public async Task UpdatePlayers()
        {
            //Mapping Brand names 
            await MapBrandNames();

            //Retrieve Player checklist from Enteractive's SDK asynchronously
            var playerCheckList = await EnteractiveClient.PlayersBulk.GetPlayerCheckList();

            //Retrieve Data of players in check list
            var playerData = GetData(playerCheckList);

            SyncPlayersRequest syncPlayersRequest = new SyncPlayersRequest
            {
                SyncPlayers = new List<SDK.Models.V2.Requests.SyncPlayer>()
            };

            List<UpdatePlayer> playersToClose = new List<UpdatePlayer>();

            //Traverse data file 
            foreach (var playerDataRow in playerData)
            {
                if (!playerDataRow.Eligible)
                {
                    playersToClose.Add(playerDataRow);
                }

                syncPlayersRequest.SyncPlayers.Add(RequestMapper(playerDataRow));
            }

            //Sync Players
            await SyncPlayers(syncPlayersRequest);

            //Close ineligible players
            await ClosePlayers(playersToClose);
        }

        /// <summary>
        /// Synchronizes the players' data
        /// </summary>
        /// <param name="syncPlayersRequest"></param>
        /// <returns></returns>
        public async Task SyncPlayers(SyncPlayersRequest syncPlayersRequest)
        {
            try
            {
                // Send Players Synchronization request using Enteractive's SDK
                var syncPlayersResponse = await EnteractiveClient.PlayersBulk.SyncPlayers(syncPlayersRequest);
                //Analyse Enteractive's SDK response and act accordingly 
                if (syncPlayersResponse.Success)
                {
                    Console.WriteLine($"Player data synchronized successfully");
                    //Check for converted players
                    if (syncPlayersResponse.ConvertedPlayers != null && syncPlayersResponse.ConvertedPlayers.Any())
                    {
                        Console.WriteLine($"{syncPlayersResponse.ConvertedPlayers.Count} converted players");
                        syncPlayersResponse.SaveToCsv("/Samples/CSV/ConvertedPlayers.csv");
                    }
                }
                else
                {
                    Console.WriteLine($"Player Synchronization failed with error: {syncPlayersResponse.ErrorMessage} converted players");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occoured while syncing players {ex.Message}");
            }
        }

        /// <summary>
        /// Closes the players provided
        /// </summary>
        /// <param name="playersToClose">List of players to close</param>
        /// <returns></returns>
        public async Task ClosePlayers(List<UpdatePlayer> playersToClose)
        {
            ClosePlayersRequest closePlayersRequest = new ClosePlayersRequest
            {
                ClosePlayers = new List<ClosePlayerRequest>()
            };
            ClosePlayerRequest closePlayer;
            foreach (var player in playersToClose)
            {
                closePlayer = new ClosePlayerRequest()
                {
                    BrandName = BrandNamesMap.GetValueOrDefault(player.Brand),
                    ClientUserId = player.UserId
                };
                closePlayersRequest.ClosePlayers.Add(closePlayer);
            }

            try
            {
                var closePlayersresponse = await EnteractiveClient.PlayersBulk.ClosePlayers(closePlayersRequest);

                if (closePlayersresponse.Success)
                {
                    Console.WriteLine("Players Closed Sucessfully");
                }
                else
                {
                    Console.WriteLine($"Error occoured while closing players: {closePlayersresponse.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occoured while closing players: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps Brands names with brand names found on Enteractive system. 
        /// Since strings are used to identify brands, brand names might differ on different systems.
        /// This mapper can be used to ensure that the brand names passed to Enteractive are correct and the players will be correctly associated with their respecitve brand
        /// </summary>
        /// <returns></returns>
        private async Task MapBrandNames()
        {
            var brandsResponse = await EnteractiveClient.Setting.GetBrands();
            if (brandsResponse.Success)
            {
                this.Brands = brandsResponse.ClientBrands;

                if (Brands.Contains("Bet One"))
                {
                    BrandNamesMap.Add("Bet1", "Bet One");
                }

                if (Brands.Contains("Bet Two"))
                {
                    BrandNamesMap.Add("Bet2", "Bet Two");
                }
            }
        }

        /// <summary>
        /// Imports player details into an array of SyncPlayer Objects
        /// This method should be updated to gather data from a relevant data source (database, api, csv, etc.)
        /// </summary>
        /// <param name="playerCheckList"> Player check list from Enteractive</param>
        /// <returns>Array of UpdatePlayer objects</returns>
        /// <returns></returns>
        private IEnumerable<Models.UpdatePlayer> GetData(List<SDK.Models.V2.Responses.PlayerCheckList> playerCheckList)
        {
            var player1 = new UpdatePlayer() { Brand = "Bet1", UserId = "b001", DepositDate = DateTime.Parse("26/05/2021"), FailedDepositDate = DateTime.Parse("11/04/2021"), LastLoginDate = DateTime.Parse("23/05/2021"), VoiceConsent = true, SmsConsent = true, EmailConsent = true, Eligible = true };
            var player2 = new UpdatePlayer() { Brand = "Bet1", UserId = "b002", DepositDate = DateTime.Parse("20/05/2021"), FailedDepositDate = DateTime.Parse("10/05/2021"), LastLoginDate = DateTime.Parse("21/05/2021"), VoiceConsent = true, SmsConsent = true, EmailConsent = true, Eligible = false };
            var player3 = new UpdatePlayer() { Brand = "Bet1", UserId = "b003", DepositDate = DateTime.Parse("10/05/2021"), FailedDepositDate = null, LastLoginDate = DateTime.Parse("11/05/2021"), VoiceConsent = true, SmsConsent = true, EmailConsent = false, Eligible = true };
            var player4 = new UpdatePlayer() { Brand = "Bet1", UserId = "b004", DepositDate = DateTime.Parse("12/05/2021"), FailedDepositDate = null, LastLoginDate = DateTime.Parse("26/05/2021"), VoiceConsent = true, SmsConsent = true, EmailConsent = true, Eligible = true };
            var player5 = new UpdatePlayer() { Brand = "Bet1", UserId = "b005", DepositDate = DateTime.Parse("26/05/2021"), FailedDepositDate = null, LastLoginDate = DateTime.Parse("13/05/2021"), VoiceConsent = true, SmsConsent = false, EmailConsent = false, Eligible = true };
            var player6 = new UpdatePlayer() { Brand = "Bet2", UserId = "c001", DepositDate = DateTime.Parse("13/05/2021"), FailedDepositDate = null, LastLoginDate = DateTime.Parse("13/05/2021"), VoiceConsent = true, SmsConsent = true, EmailConsent = true, Eligible = true };
            var player7 = new UpdatePlayer() { Brand = "Bet2", UserId = "c002", DepositDate = DateTime.Parse("14/05/2021"), FailedDepositDate = DateTime.Parse("12/05/2021"), LastLoginDate = DateTime.Parse("15/05/2021"), VoiceConsent = true, SmsConsent = true, EmailConsent = true, Eligible = true };
            var player8 = new UpdatePlayer() { Brand = "Bet2", UserId = "c003", DepositDate = DateTime.Parse("11/04/2021"), FailedDepositDate = DateTime.Parse("06/04/2021"), LastLoginDate = DateTime.Parse("12/04/2021"), VoiceConsent = true, SmsConsent = true, EmailConsent = false, Eligible = false };
            var player9 = new UpdatePlayer() { Brand = "Bet2", UserId = "c004", DepositDate = DateTime.Parse("16/03/2021"), FailedDepositDate = null, LastLoginDate = DateTime.Parse("17/03/2021"), VoiceConsent = false, SmsConsent = true, EmailConsent = true, Eligible = true };
            var player10 = new UpdatePlayer() { Brand = "Bet2", UserId = "c005", DepositDate = DateTime.Parse("16/05/2021"), FailedDepositDate = DateTime.Parse("07/04/2021"), LastLoginDate = DateTime.Parse("16/05/2021"), VoiceConsent = false, SmsConsent = false, EmailConsent = false, Eligible = true };

            List<UpdatePlayer> playerData = new List<Models.UpdatePlayer>
            {
                player1,
                player2,
                player3,
                player4,
                player5,
                player6,
                player7,
                player8,
                player9,
                player10
            };

            List<UpdatePlayer> playersNotInCheckList = new List<Models.UpdatePlayer>();
            //Get players which are in Player Check List
            foreach (var dataRow in playerData)
            {
                bool playerExistsInCheckList = playerCheckList.Any(p =>
                        p.ClientUserId == dataRow.UserId // Compare player id
                        && p.BrandName.Equals(BrandNamesMap.GetValueOrDefault(dataRow.Brand), StringComparison.OrdinalIgnoreCase) //Compare Player's Brand using Brand mapping
                        );

                //If player is not present in Player Check List, remove it
                if (!playerExistsInCheckList)
                {
                    playersNotInCheckList.Add(dataRow);
                }
            }

            return playerData.Except(playersNotInCheckList);
        }

        /// <summary>
        /// Maps the data retrieved from CSV file into the Request object utilized by Enteractive's SDK
        /// </summary>
        /// <param name="playerData"></param>
        /// <returns>Sync Player Object</returns>
        private SDK.Models.V2.Requests.SyncPlayer RequestMapper(Models.UpdatePlayer playerData)
        {
            var player = new SDK.Models.V2.Requests.SyncPlayer()
            {
                BrandName = BrandNamesMap.GetValueOrDefault(playerData.Brand), //Map brand name
                ClientUserId = playerData.UserId,
                LastDepositDate = playerData.DepositDate,
                FailedDepositDate = playerData.FailedDepositDate,
                LastLoginDate = playerData.LastLoginDate,
                VoiceConsent = playerData.VoiceConsent,
                SmsConsent = playerData.SmsConsent,
                EmailConsent = playerData.EmailConsent,
                LastSyncDate = DateTime.Now
            };

            return player;
        }
    }
}
