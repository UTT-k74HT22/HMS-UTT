﻿namespace HospitalManagement.entity
{
    /// <summary>
    /// CustomerProfile entity - extends UserProfile with customer-specific info
    /// </summary>
    public class CustomerProfile : BaseEntity
    {
        /// <summary>
        /// Reference to UserProfile
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        /// Customer type: RETAIL, WHOLESALE
        /// </summary>
        public string CustomerType { get; set; } = string.Empty; 

        /// <summary>
        /// Tax identification code
        /// </summary>
        public string? TaxCode { get; set; }
    }
}

