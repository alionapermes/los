using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class UserOrdersModel : PageModel
{
    private NpgsqlDataSource _dataSource;

    public UserOrdersModel()
    {
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public void OnGet()
    {
    }

    public List<Order> GetAllOrders()
    {
        return OrdersData.GetAll(_dataSource);
    }
}