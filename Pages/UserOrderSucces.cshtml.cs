using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class UserOrderSuccessModel : PageModel
{
    public Int64 OrderID{get; set;}
    public string? SecretCode{get; set;}
    private NpgsqlDataSource _dataSource;

    public UserOrderSuccessModel()
    {
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public IActionResult OnGet()
    {
      Int64 orderID = 0;
      if (!Int64.TryParse(Request.Query["id"], out orderID)) {
        return Redirect("/UserInterface");
      }
      OrderID = orderID;

      SecretCode = Request.Query["secret_code"];
      if (string.IsNullOrEmpty(SecretCode)) {
        return Redirect("/UserInterface");
      }

      return Page();
    }
}