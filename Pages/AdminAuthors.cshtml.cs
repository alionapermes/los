using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class AdminAuthorsModel : PageModel
{
    public string? ActionType{get; set;}
    public Int64? InputID{get; set;}
    public string? InputName{get; set;}

    private NpgsqlDataSource _dataSource;

    public AdminAuthorsModel()
    {
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public void OnPost()
    {
        Console.WriteLine("AdminAuthorsModel.ActionType: " + ActionType);
        switch (ActionType) {
            case "insert": this.addAuthor(); break;
            case "remove": this.removeAuthor(); break;
            default: return;
        }
    }

    public List<Author> GetAllAuthors()
    {
        return AuthorsData.GetAll(_dataSource);
    }

    private void addAuthor()
    {
        if (string.IsNullOrEmpty(InputName)) {
            Console.WriteLine("addAuthor: epmty name");
            return;
        }

        var sql = "INSERT INTO author (fullname) VALUES (@fullname)";

        using (var cmd = _dataSource.CreateCommand(sql)) {
            cmd.Parameters.AddWithValue("fullname", InputName);
            cmd.ExecuteNonQuery();
        }
    }

    public void removeAuthor()
    {
        Int64 authorID = InputID ?? 0;
        if (authorID <= 0) {
            Console.WriteLine("removeAuthor: incorrect author id");
            return;
        }

        var sql = "DELETE FROM author WHERE id = @id";

        using (var cmd = _dataSource.CreateCommand(sql)) {
            cmd.Parameters.AddWithValue("id", authorID);
            cmd.ExecuteNonQuery();
        }
    }
}