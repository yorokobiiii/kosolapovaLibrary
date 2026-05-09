using LibraryOnline.Models;
using LibraryOnline.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Подключение к БД из appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(connectionString));
// Регистрация сервиса
builder.Services.AddScoped<IBookService, BookService>();
var app = builder.Build();
// Инициализация БД с тестовыми данными
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    db.Database.EnsureCreated();

    if (!db.Books.Any())
    {
        db.Books.AddRange(
            new Book { Title = "Война и мир", YearPublished = 1869, Price = 399, FileFormat = "FB2" },
            new Book { Title = "Преступление и наказание", YearPublished = 1866, Price = 449, FileFormat = "EPUB" },
            new Book { Title = "Мастер и Маргарита", YearPublished = 1967, Price = 479, FileFormat = "PDF" }
        );
        db.SaveChanges();
    }

    if (!db.Readers.Any())
    {
        db.Readers.AddRange(
            new Reader { FirstName = "Анна", LastName = "Дробжева", Email = "drobzheva@mail.ru", RegistrationDate = DateTime.Today.AddDays(-45), IsPremium = true },
            new Reader { FirstName = "Виктория", LastName = "Верховых", Email = "verhovyh@mail.ru", RegistrationDate = DateTime.Today.AddDays(-20), IsPremium = true },
            new Reader { FirstName = "Виктория", LastName = "Косолапова", Email = "kosolapova@mail.ru", RegistrationDate = DateTime.Today.AddDays(-10), IsPremium = false }
        );
        db.SaveChanges();
    }
}
// ===== ЭНДПОИНТЫ =====
// GET /api/books
app.MapGet("/api/books", async (LibraryContext db, IBookService service) =>
{
    var books = await db.Books.ToListAsync(); 
    var result = books.Select(b => service.Format(b));
    var appName = app.Configuration["AppSettings:AppName"];
    var version = app.Configuration["AppSettings:Version"];
    return Results.Json(new
    {
        appName = appName,
        version = version,
        totalBooks = result.Count(),
        data = result
    });
});
// GET /api/books/{id}
app.MapGet("/api/books/{id:int}", async (int id, LibraryContext db) =>
{
    var book = await db.Books.FindAsync(id);
    if (book == null) return Results.NotFound(new { error = "Книга не найдена" });
    
    return Results.Json(new
    {
        book.BookId,
        book.Title,
        book.YearPublished,
        book.Price,
        book.FileFormat,
        book.Rating
    });
});
// POST /api/books
app.MapPost("/api/books", async (Book newBook, LibraryContext db) =>
{
    if (string.IsNullOrWhiteSpace(newBook.Title))
        return Results.BadRequest(new { error = "Название книги обязательно" });
    
    db.Books.Add(newBook);
    await db.SaveChangesAsync();
    return Results.Created($"/api/books/{newBook.BookId}", newBook);
});
// GET /api/readers
app.MapGet("/api/readers", async (LibraryContext db) =>
{
    var readers = await db.Readers.ToListAsync();
    return Results.Json(new
    {
        totalReaders = readers.Count(),
        data = readers.Select(r => new
        {
            r.ReaderId,
            FullName = r.FullName,
            r.FirstName,
            r.LastName,
            r.Email,
            r.Phone,
            RegistrationDate = r.RegistrationDate.ToString("dd.MM.yyyy"),
            r.IsPremium
        })
    });
});
// GET /api/readers/{id}
app.MapGet("/api/readers/{id:int}", async (int id, LibraryContext db) =>
{
    var reader = await db.Readers.FindAsync(id);
    if (reader == null) return Results.NotFound(new { error = "Читатель не найден" });
    return Results.Json(new
    {
        reader.ReaderId,
        FullName = reader.FullName,
        reader.FirstName,
        reader.LastName,
        reader.Email,
        reader.Phone,
        RegistrationDate = reader.RegistrationDate.ToString("dd.MM.yyyy"),
        reader.IsPremium
    });
});
// GET /api/config
app.MapGet("/api/config", (IConfiguration config) =>
{
    return Results.Json(new
    {
        appName = config["AppSettings:AppName"],
        version = config["AppSettings:Version"],
        maxItems = config["AppSettings:MaxItems"],
        dbProvider = "SQLite",
        endpoints = new[] { 
            "GET /api/books", 
            "GET /api/books/{id}", 
            "POST /api/books", 
            "GET /api/readers",
            "GET /api/readers/{id}",
            "GET /api/config" 
        }
    });
});
// Стартовая страница
app.MapGet("/", () => "=== Library API ===\nЧитатели: Анна Дробжева, Виктория Верховых, Виктория Косолапова\n\nДоступные endpoint'ы:\nGET  /api/books\nGET  /api/books/{id}\nPOST /api/books\nGET  /api/readers\nGET  /api/readers/{id}\nGET  /api/config");
app.Run();