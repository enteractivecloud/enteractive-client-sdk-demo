using System;

namespace Enteractive.SDK.Demo.Models
{
    /// <summary>
    /// Model used to get data for player import. 
    /// This class represents one row of data. 
    /// </summary>
    public class ImportPlayer
    {
        /// <summary>
        /// Player brand name
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Unique identifier for this player
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Player's username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Player's firstname
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Player's lastname
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Player's mobile number
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Player's Country code
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Player's last deposit date required for reactivation and VFC
        /// </summary>
        public DateTime? DepositDate { get; set; }

        /// <summary>
        /// Player's registration date required for NRC
        /// </summary>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Player's last login date
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Players attributes
        /// Allowed attributes: Sports, Casino, Poker, Bingo, Lotto, BetUP
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// Value indicating if player is to be contacted by voice calls
        /// </summary>
        public bool VoiceConsent { get; set; }

        /// <summary>
        /// Value indicating if player is to be contacted by sms
        /// </summary>
        public bool SmsConsent { get; set; }

        /// <summary>
        /// Player's AffiliateCode code
        /// </summary>
        public string AffiliateCode { get; set; }
    }
}
