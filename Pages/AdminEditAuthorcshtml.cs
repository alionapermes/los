using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class AdminEditAuthorModel : PageModel
{
    public string? ErrMsg{get; set;}
    public Int64? InputID{get; set;}
    public string? InputFullname{get; set;}
    private NpgsqlDataSource _dataSource;

    public AdminEditAuthorModel()
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

        var author = AuthorsData.GetByID(_dataSource, id);

        InputID = id;
        InputFullname = author.Fullname;
    }

    public IActionResult OnPost()
    {
        Int64 authorID = InputID ?? 0;
        if (authorID <= 0) {
            Console.WriteLine("AdminEditAuthor.OnPost: incorrect id");
            return new EmptyResult();
        }

        var author = AuthorsData.GetByID(_dataSource, authorID);

        if (string.IsNullOrEmpty(InputFullname)) {
            Console.WriteLine("AdminEditAuthor.OnPost: incorrect author name");
            return new EmptyResult();
        }

        author.Fullname = InputFullname;
        AuthorsData.Update(_dataSource, author);

        return Redirect("/AdminAuthors");
    }
}