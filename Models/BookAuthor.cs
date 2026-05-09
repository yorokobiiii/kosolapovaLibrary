using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LibraryOnline.Models
{
    [Table("BookAuthors")]
    public class BookAuthor
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        [StringLength(50)]
        public string Role { get; set; } = "Автор"; // Автор, Составитель, Переводчик
        // Навигационные свойства
        public  Book Book { get; set; }
        public  Author Author { get; set; }
    }
}