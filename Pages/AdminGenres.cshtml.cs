using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class AdminGenresModel : PageModel
{
    public string? ActionType{get; set;}
    public Int64? InputID{get; set;}
    public string? InputTitle{get; set;}

    private NpgsqlDataSource _dataSource;

    public AdminGenresModel()
    {
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public void OnPost()
    {
        Console.WriteLine("AdminGenresModel.ActionType: " + ActionType);
        switch (ActionType) {
            case "insert": this.addGenre(); break;
            case "remove": this.removeGenre(); break;
            default: return;
        }
    }

    public List<Genre> GetAllGenres()
    {
        return GenresData.GetAll(_dataSource);
    }

    private void addGenre()
    {
        if (string.IsNullOrEmpty(InputTitle)) {
            Console.WriteLine("addGenre: epmty title");
            return;
        }

        var sql = "INSERT INTO genre (title) VALUES (@title)";

        using (var cmd = _dataSource.CreateCommand(sql)) {
            cmd.Parameters.AddWithValue("title", InputTitle);
            cmd.ExecuteNonQuery();
        }
    }

    public void removeGenre()
    {
        Int64 genreID = InputID ?? 0;
        if (genreID <= 0) {
            Console.WriteLine("removeGenre: incorrect genre id");
            return;
        }

        var sql = "DELETE FROM genre WHERE id = @id";

        using (var cmd = _dataSource.CreateCommand(sql)) {
            cmd.Parameters.AddWithValue("id", genreID);
            cmd.ExecuteNonQuery();
        }
    }
}