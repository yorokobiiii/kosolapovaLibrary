using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LibraryOnline.Models
{
    [Table("Authors")]
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? LastName { get; set; }
        [StringLength(300)]
        public string? Biography { get; set; }
        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }
        // Навигационное свойство
        public ICollection<BookAuthor> BookAuthors { get; set; }
        [NotMapped]
        public string FullName => $"{LastName} {FirstName}";
    }
}
