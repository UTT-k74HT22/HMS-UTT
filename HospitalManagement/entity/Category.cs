﻿namespace HospitalManagement.entity
{
    /// <summary>
    /// Category entity - represents product categories (supports hierarchy)
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// Unique category code
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// Category name
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Category description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Parent category ID (for hierarchy)
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Is category active/visible
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Display order for sorting
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}

