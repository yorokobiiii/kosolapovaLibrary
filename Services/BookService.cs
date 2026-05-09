using LibraryOnline.Models;
namespace LibraryOnline.Services;
public class BookService : IBookService
{
    public object Format(Book book)
    {
        return new
        {
            book.BookId,
            book.Title,
            Year = book.YearPublished,
            book.ISBN,
            book.Price,
            book.FileFormat,
            book.DownloadCount,
            book.Rating,
            IsExpensive = book.Price > 500,     
            ShortInfo = book.ShortInfo            
        };
    }
}