using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class AdminEditGenreModel : PageModel
{
    public string? ErrMsg{get; set;}
    public Int64? InputID{get; set;}
    public string? InputTitle{get; set;}
    private NpgsqlDataSource _dataSource;

    public AdminEditGenreModel()
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

        var genre = GenresData.GetByID(_dataSource, id);

        InputID = id;
        InputTitle = genre.Title;
    }

    public IActionResult OnPost()
    {
        Int64 genreID = InputID ?? 0;
        if (genreID <= 0) {
            Console.WriteLine("AdminEditGenre.OnPost: incorrect id");
            return new EmptyResult();
        }

        var genre = GenresData.GetByID(_dataSource, genreID);

        if (string.IsNullOrEmpty(InputTitle)) {
            Console.WriteLine("AdminEditGenre.OnPost: incorrect genre title");
            return new EmptyResult();
        }

        genre.Title = InputTitle;
        GenresData.Update(_dataSource, genre);

        return Redirect("/AdminGenres");
    }
}