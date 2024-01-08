using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Util;

namespace los.Pages;

[BindProperties]
public class AdminEditBookModel : PageModel
{
    public string? ErrMsg{get; set;}
    public Int64? InputID{get; set;}
    public string? InputTitle{get; set;}

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

        var book = Data.GetBookByID(_dataSource, id);

        InputID = id;
        InputTitle = book.Title;
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

        Data.UpdateBook(_dataSource, new Book(bookID, InputTitle));
        return Redirect("/AdminBooks");
    }

    public List<Book> GetAllBooks()
    {
        return Data.GetAllBooks(_dataSource);
    }
}