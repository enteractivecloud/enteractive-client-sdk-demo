using System;

namespace Enteractive.SDK.Demo.Models
{
    /// <summary>
    /// Model used to get data for player synchronization. 
    /// This class represents one row of data. 
    /// </summary>
    public class UpdatePlayer
    {
        /// <summary>
        /// Player's brand name
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Unique identifier for this player
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The date when the player last made a deposit
        /// </summary>
        public DateTime DepositDate { get; set; }

        /// <summary>
        /// The date when the player last made a failed deposit
        /// </summary>
        public DateTime? FailedDepositDate { get; set; }

        /// <summary>
        /// The date when the player last logged in to the system
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// True when the player allows to be contacted by calls
        /// </summary>
        public bool VoiceConsent { get; set; }

        /// <summary>
        /// True if the player allows to be contacted by sms
        /// </summary>
        public bool SmsConsent { get; set; }

        /// <summary>
        /// True if the player allows to be contacted by email
        /// </summary>
        public bool? EmailConsent { get; set; }

        /// <summary>
        /// True if the player satisfies the appropriate conditions
        /// </summary>
        public bool Eligible { get; set; }

    }
}
