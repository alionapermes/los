using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class AdminBooksModel : PageModel
{
    public string? ActionType{get; set;}
    public Int64? InputID{get; set;}
    public string? InputTitle{get; set;}
    public Int32? InputYear{get; set;}
    public Int64? InputGenreID{get; set;}
    public Int64? InputAuthorID{get; set;}

    private NpgsqlDataSource _dataSource;

    public AdminBooksModel()
    {
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public void OnPost()
    {
        Console.WriteLine("AdminBooksModel.ActionType: " + ActionType);
        switch (ActionType) {
            case "insert": this.addBook(); break;
            case "remove": this.removeBook(); break;
            default: return;
        }
    }

    public List<Book> GetAllBooks()
    {
        return BooksData.GetAll(_dataSource);
    }

    public List<Genre> GetAllGenres()
    {
        return GenresData.GetAll(_dataSource);
    }

    public List<Author> GetAllAuthors()
    {
        return AuthorsData.GetAll(_dataSource);
    }

    private void addBook()
    {
        Genre? genre = null;
        Author author;

        if (string.IsNullOrEmpty(InputTitle)) {
            Console.WriteLine("addBook: epmty title");
            return;
        }

        Int32 year = InputYear ?? 0;
        if (year > DateTime.Today.Year) {
            Console.WriteLine("AdminEditBook.OnPost: incorrect year");
            return;
        }

        Int64 authorID = InputAuthorID ?? 0;
        if (authorID <= 0) {
            Console.WriteLine("AdminEditBook.OnPost: incorrect author id");
            return;
        }

        Int64 genreID = InputGenreID ?? 0;
        if (genreID > 0) {
            genre = GenresData.GetByID(_dataSource, genreID);
        }

        author = AuthorsData.GetByID(_dataSource, authorID);
        var book = new Book(InputTitle, year, author, genre);

        BooksData.Add(_dataSource, book);
    }

    public void removeBook()
    {
        Int64 bookID = InputID ?? 0;
        if (bookID <= 0) {
            Console.WriteLine("removeBook: incorrect book id");
            return;
        }

        BooksData.DeleteByID(_dataSource, bookID);
    }
}