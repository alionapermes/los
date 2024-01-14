using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class UserInterfaceModel : PageModel
{
    public string? ErrMsg{get; set;}
    public string? InputUserName{get; set;}
    private NpgsqlDataSource _dataSource;

    public UserInterfaceModel()
    {
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(this.InputUserName)) {
            this.ErrMsg = "empty username";
            return new EmptyResult();
        }

        User user = UsersData.FindByName(_dataSource, this.InputUserName);
        if (user == null) {
            user = new User(this.InputUserName);
            Int64 userID = UsersData.Add(_dataSource, user);
            return Redirect($"/UserOrders?user_id={userID}");
        }

        return Redirect($"/UserOrders?user_id={user.ID}");
    }
}