using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class AdminEditBookModel : PageModel
{
    public string? ErrMsg{get; set;}
    public Int64? InputID{get; set;}
    public string? InputTitle{get; set;}
    public Int32? InputYear{get; set;}
    public Int64? InputGenreID{get; set;}
    public Int64? InputAuthorID{get; set;}

    private NpgsqlDataSource _dataSource;

    public AdminEditBookModel()
    {
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public void OnGet()
    {
        Int64 id;
        if (!Int64.TryParse(Request.Query["id"], out id)) {
            return;
        }

        var book = BooksData.GetByID(_dataSource, id);

        InputID = id;
        InputTitle = book.Title;
        InputYear = book.Year;
        InputGenreID = book.Genre?.ID;
        InputAuthorID = book.Author.ID;
    }

    public IActionResult OnPost()
    {
        Int64 bookID = InputID ?? 0;
        if (bookID <= 0) {
            Console.WriteLine("AdminEditBook.OnPost: incorrect id");
            return new EmptyResult();
        }

        if (string.IsNullOrEmpty(InputTitle)) {
            Console.WriteLine("AdminEditBook.OnPost: incorrect title");
            return new EmptyResult();
        }

        Int32 year = InputYear ?? 0;
        if (year > DateTime.Today.Year) {
            Console.WriteLine("AdminEditBook.OnPost: incorrect year");
            return new EmptyResult();
        }

        Int64 genreID = InputGenreID ?? 0;
        if (genreID <= 0) {
            Console.WriteLine("AdminEditBook.OnPost: incorrect genre id");
            return new EmptyResult();
        }

        Int64 authorID = InputAuthorID ?? 0;
        if (authorID <= 0) {
            Console.WriteLine("AdminEditBook.OnPost: incorrect author id");
            return new EmptyResult();
        }

        var genre = new Genre(genreID, string.Empty);
        var author = new Author(authorID, string.Empty);
        var book = new Book(bookID, InputTitle, year, author, genre);
        BooksData.Update(_dataSource, book);

        return Redirect("/AdminBooks");
    }

    public List<Genre> GetAllGenres()
    {
        return GenresData.GetAll(_dataSource);
    }

    public List<Author> GetAllAuthors()
    {
        return AuthorsData.GetAll(_dataSource);
    }
}