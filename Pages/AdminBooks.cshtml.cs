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
        if (string.IsNullOrEmpty(InputTitle)) {
            Console.WriteLine("addBook: epmty title");
            return;
        }

        var sql = "INSERT INTO book (title) VALUES (@title)";

        using (var cmd = _dataSource.CreateCommand(sql)) {
            cmd.Parameters.AddWithValue("title", InputTitle);
            cmd.ExecuteNonQuery();
        }
    }

    public void removeBook()
    {
        Int64 bookID = InputID ?? 0;
        if (bookID <= 0) {
            Console.WriteLine("removeBook: incorrect book id");
            return;
        }

        var sql = "DELETE FROM book WHERE id = @id";

        using (var cmd = _dataSource.CreateCommand(sql)) {
            cmd.Parameters.AddWithValue("id", bookID);
            cmd.ExecuteNonQuery();
        }
    }
}