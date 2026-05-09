using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LibraryOnline.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        [StringLength(200)]
        public string? Title { get; set; }
        [StringLength(50)]
        public string? ISBN { get; set; }
        [Range(1000, 9999)]
        public int YearPublished { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        // Для онлайн-библиотеки: формат файла и размер
        [StringLength(20)]
        public string FileFormat { get; set; } = "PDF"; // PDF, EPUB, FB2
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; } = 0.00m;
        public long FileSizeBytes { get; set; }
        public string? FilePath { get; set; } // путь к файлу на сервере
        public int DownloadCount { get; set; } = 0;
        public decimal Rating { get; set; } = 0.0m;
        // Навигационные свойства
        public  ICollection<BookAuthor> BookAuthors { get; set; }
        [NotMapped]
        public string ShortInfo => $"{Title} ({YearPublished})";
    }
}