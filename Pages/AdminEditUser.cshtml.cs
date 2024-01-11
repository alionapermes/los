using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class AdminEditUserModel : PageModel
{
    public string? ErrMsg{get; set;}
    public Int64? InputID{get; set;}
    public string? InputName{get; set;}
    private NpgsqlDataSource _dataSource;

    public AdminEditUserModel()
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

        var user = UsersData.GetByID(_dataSource, id);

        InputID = id;
        InputName = user.Name;
    }

    public IActionResult OnPost()
    {
        Int64 userID = InputID ?? 0;
        if (userID <= 0) {
            Console.WriteLine("AdminEditUser.OnPost: incorrect id");
            return new EmptyResult();
        }

        var user = UsersData.GetByID(_dataSource, userID);

        if (string.IsNullOrEmpty(InputName)) {
            Console.WriteLine("AdminEditUser.OnPost: incorrect user name");
            return new EmptyResult();
        }

        user.Name = InputName;
        UsersData.Update(_dataSource, user);

        return Redirect("/AdminUsers");
    }
}