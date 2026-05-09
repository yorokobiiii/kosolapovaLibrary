using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LibraryOnline.Models
{
    [Table("Readers")]
    public class Reader
    {
        [Key]
        public int ReaderId { get; set; }
        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }
        [StringLength(150)]
        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(20)]
        [Phone]
        public string? Phone { get; set; }
        [Column(TypeName = "date")]
        public DateTime RegistrationDate { get; set; } = DateTime.Today;
        public bool IsPremium { get; set; } = false; // доступ к платному контенту
        public DateTime? PremiumUntil { get; set; }
        [NotMapped]
        public string FullName => $"{LastName} {FirstName}";
    }
}