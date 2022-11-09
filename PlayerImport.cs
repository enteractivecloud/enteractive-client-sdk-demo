using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enteractive.SDK.Demo.Models;
using Enteractive.SDK.Models.V2.Requests;
using Enteractive.SDK.Models.V2.Responses;

namespace Enteractive.SDK.Demo
{
    /// <summary>
    /// This class demonstrates the process of importing new players
    /// </summary>
    public class PlayerImport
    {
        private List<string> brands;
        private List<CallProject> callProjects;
        private readonly Dictionary<string, string> brandNamesMap = new Dictionary<string, string>();

        enum CampaignType
        {
            Reactivation,
            NRC,
            VFC,
        }

        /// <summary>
        /// Players import process
        /// </summary>
        /// <returns></returns>
        public async Task ImportPlayers()
        {
            //Mapping Brand names 
            await MapBrandNames();

            //Set up Campaign Type
            string campaignType = CampaignType.Reactivation.ToString();

            //Retrieving data
            var playerData = GetData();
            Console.WriteLine($"{playerData.Count()} players loaded");

            //Validate if call project for the brand, country and campaign type exists and filter list accordingly 
            playerData = await ValidatePlayerData(playerData, campaignType);

            //Mapping Request object
            var players = RequestMapper(playerData);

            AddPlayersRequest addPlayersRequest = new AddPlayersRequest()
            {
                CampaignType = campaignType,
                Players = players
            };

            try
            {
                // Calling Enteractive's SDK asynchronously
                var addPlayersResponse = await EnteractiveClient.Player.AddPlayers(addPlayersRequest);

                //Analyse response from Enteractive's SDK and act accordingly 
                if (addPlayersResponse.Success)
                {
                    Console.WriteLine($"{addPlayersResponse.PlayersImported.Count} Players Imported Sucessfully");
                    Console.WriteLine($"{addPlayersResponse.PlayersRejected.Count} Players Rejected");
                }
                else
                {
                    Console.WriteLine($"Players Import failed: {addPlayersResponse.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occoured while importing players {ex.Message}");
            }
        }

        /// <summary>
        /// Imports Player Data into an array of ImportPlayer Objects
        /// This method should be updated to gather data from a relevant data source (database, api, csv, etc.)
        /// </summary>
        /// <returns>Array of ImportPlayer objects</returns>
        private IEnumerable<ImportPlayer> GetData()
        {
            var player1 = new ImportPlayer() { Brand = "Bet1", UserId = "b001", Username = "JohnDoe", FirstName = "John", LastName = "Doe", Mobile = "35679000001", Country = "fi", DepositDate = new DateTime(2021, 5, 20), Attribute = "Sports", VoiceConsent = true, SmsConsent = true };
            var player2 = new ImportPlayer() { Brand = "Bet1", UserId = "b002", Username = "jtaylor", FirstName = "Jane", LastName = "Taylor", Mobile = "35679000002", Country = "fi", DepositDate = new DateTime(2021, 5, 18), Attribute = "Casino", VoiceConsent = true, SmsConsent = false };
            var player3 = new ImportPlayer() { Brand = "Bet1", UserId = "b003", Username = "JoeSmith", FirstName = "Joseph", LastName = "Smith", Mobile = "35679000003", Country = "gb", DepositDate = new DateTime(2021, 5, 10), Attribute = "Poker", VoiceConsent = true, SmsConsent = true };
            var player4 = new ImportPlayer() { Brand = "Bet1", UserId = "b004", Username = "KurtM", FirstName = "Kurt", LastName = "Morrison", Mobile = "35679000004", Country = "gb", DepositDate = new DateTime(2021, 5, 11), Attribute = "Sports", VoiceConsent = true, SmsConsent = true };
            var player5 = new ImportPlayer() { Brand = "Bet1", UserId = "b005", Username = "hjones", FirstName = "George", LastName = "Jones", Mobile = "35679000005", Country = "gb", DepositDate = new DateTime(2021, 5, 12), Attribute = "Lotto", VoiceConsent = false, SmsConsent = false };
            var player6 = new ImportPlayer() { Brand = "Bet2", UserId = "c001", Username = "otaylor", FirstName = "Oliver", LastName = "Taylor", Mobile = "35679000006", Country = "se", DepositDate = new DateTime(2021, 5, 13), Attribute = "Bingo", VoiceConsent = true, SmsConsent = true };
            var player7 = new ImportPlayer() { Brand = "Bet2", UserId = "c002", Username = "hbrown", FirstName = "Harry", LastName = "Brown", Mobile = "35679000007", Country = "fi", DepositDate = new DateTime(2021, 5, 14), Attribute = "Sports", VoiceConsent = true, SmsConsent = true };
            var player8 = new ImportPlayer() { Brand = "Bet2", UserId = "c003", Username = "jwilliams", FirstName = "Jack", LastName = "Williams", Mobile = "35679000008", Country = "gb", DepositDate = new DateTime(2021, 4, 11), Attribute = "Sports", VoiceConsent = true, SmsConsent = false };
            var player9 = new ImportPlayer() { Brand = "Bet2", UserId = "c004", Username = "cjohnson", FirstName = "Charlie", LastName = "Johnson", Mobile = "35679000009", Country = "se", DepositDate = new DateTime(2021, 3, 16), Attribute = "Lotto", VoiceConsent = true, SmsConsent = true };
            var player10 = new ImportPlayer() { Brand = "Bet2", UserId = "c005", Username = "edavies", FirstName = "Emily", LastName = "Davies", Mobile = "356790000010", Country = "se", DepositDate = new DateTime(2021, 5, 16), Attribute = "Poker", VoiceConsent = true, SmsConsent = false };

            List<ImportPlayer> playerData = new List<ImportPlayer>
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

            return playerData;
        }

        /// <summary>
        /// Maps Brands names with brand names found on Enteractive system. 
        /// Since strings are used to identify brands, brand names might differ on different systems.
        /// This mapper can be used to ensure that the brand names passed to Enteractive are correct and the players will be correctly associated with their respective brand
        /// </summary>
        /// <returns></returns>
        private async Task MapBrandNames()
        {
            if (this.brands == null)
            {
                var brandsResponse = await EnteractiveClient.Setting.GetBrands();
                if (brandsResponse.Success)
                {
                    this.brands = brandsResponse.ClientBrands;

                    if (brands.Contains("Bet One"))
                    {
                        brandNamesMap.Add("Bet1", "Bet One");
                    }

                    if (brands.Contains("Bet Two"))
                    {
                        brandNamesMap.Add("Bet2", "Bet Two");
                    }
                }
            }
        }

        /// <summary>
        /// Maps the Csv Data into the Request object utilized by Enteractive's SDK
        /// </summary>
        /// <param name="playerData">Data imported from csv</param>
        /// <returns>Array of player request object</returns>
        private List<SDK.Models.V2.Requests.Player> RequestMapper(IEnumerable<ImportPlayer> playerData)
        {
            List<SDK.Models.V2.Requests.Player> playerList = new List<SDK.Models.V2.Requests.Player>();

            foreach (var playerDetails in playerData)
            {
                var player = new SDK.Models.V2.Requests.Player
                {
                    BrandName = brandNamesMap.GetValueOrDefault(playerDetails.Brand), //map brand name
                    ClientUserId = playerDetails.UserId,
                    UserName = playerDetails.Username,
                    FirstName = playerDetails.FirstName,
                    LastName = playerDetails.LastName,
                    Mobile = playerDetails.Mobile,
                    CountryISO2 = playerDetails.Country,
                    LastDepositDate = playerDetails.DepositDate, //Required for Reactivation and VFC
                    RegistrationDate = playerDetails.RegistrationDate, //Required for NRC
                    AdditionalAttributes = new List<string>
                    {
                        playerDetails.Attribute
                    },
                    AffiliateCode = playerDetails.AffiliateCode,
                    VoiceConsent = playerDetails.VoiceConsent,
                    SmsConsent = playerDetails.SmsConsent
                };
                playerList.Add(player);

            }
            return playerList;
        }

        /// <summary>
        /// Validates that a Call Project for the player being imported exists. 
        /// Call Projects are identified by Brand, Country and Campaign Type
        /// </summary>
        /// <param name="brand">Player's brand name</param>
        /// <param name="country">Player's Country code</param>
        /// <param name="campaignType"> Campaign type</param>
        /// <returns>Boolean</returns>
        private async Task<bool> ValidateCallProject(string brand, string country, string campaignType)
        {
            if (callProjects == null)
            {
                var callProjectsResponse = await EnteractiveClient.Setting.GetCallProjects();
                if (callProjectsResponse.Success)
                {
                    callProjects = callProjectsResponse.CallProjects;
                }
            }

            if (callProjects.Any(cp => cp.BrandName.Equals(brand, StringComparison.OrdinalIgnoreCase) && cp.CountryISO2 == country && cp.CampaignType == campaignType))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Filters the player data being imported from any Brands or Countries that do not have a Call Project set up.
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="campaignType"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ImportPlayer>> ValidatePlayerData(IEnumerable<ImportPlayer> playerData, string campaignType)
        {
            var filteredPlayerData = new List<ImportPlayer>();
            // Get distinct Brand and Country combinations 
            foreach (var callProject in playerData.Select(p => new { p.Brand, p.Country }).Distinct())
            {
                // CHeck if a call project exists for the Brand Country combination
                if ((await ValidateCallProject(brandNamesMap.GetValueOrDefault(callProject.Brand), callProject.Country, campaignType)))
                {
                    //If call project exists add the players which belong to that Brand and Country
                    filteredPlayerData.AddRange(playerData.Where(p => p.Brand == callProject.Brand && p.Country == callProject.Country));
                }
                else
                {
                    Console.WriteLine($"{campaignType} Call Project does not exists for brand: {callProject.Brand} and country: {callProject.Country}");
                }
            }

            //return remaining players 
            return filteredPlayerData;
        }

    }
}
