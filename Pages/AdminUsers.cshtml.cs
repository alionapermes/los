using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class AdminUsersModel : PageModel
{
    public string? ActionType{get; set;}
    public Int64? InputID{get; set;}
    public string? InputName{get; set;}

    private NpgsqlDataSource _dataSource;

    public AdminUsersModel()
    {
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public void OnPost()
    {
        Console.WriteLine("AdminUsersModel.ActionType: " + ActionType);
        switch (ActionType) {
            case "insert": this.addUser(); break;
            case "remove": this.removeUser(); break;
            default: return;
        }
    }

    public List<User> GetAllUsers()
    {
        return UsersData.GetAll(_dataSource);
    }

    private void addUser()
    {
        if (string.IsNullOrEmpty(InputName)) {
            Console.WriteLine("addUser: epmty name");
            return;
        }

        UsersData.Add(_dataSource, new User(InputName));
    }

    public void removeUser()
    {
        Int64 userID = InputID ?? 0;
        if (userID <= 0) {
            Console.WriteLine("removeUser: incorrect user id");
            return;
        }

        UsersData.DeleteByID(_dataSource, userID);
    }
}
