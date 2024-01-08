using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;

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
        var orders = new List<Order>();

        var sql =
            "SELECT " +
                "o.id, o.username, b.id as book_id, b.title as book_title, " +
                "o.ordered_on, o.arrives_on " +
            "FROM order LEFT ";

        using (var cmd = _dataSource.CreateCommand(sql))
        using (var reader = cmd.ExecuteReader()) {
            while (reader.Read()) {
                var id = reader.GetInt64(0);
                var userName = reader.GetString(1);
                var bookID = reader.GetInt64(2);
                var bookTitle = reader.GetString(3);
                var orderedOn = reader.GetDateTime(4);
                var arrivesOn = reader.GetDateTime(5);

                var book = new Book(bookID, bookTitle);
                var order = new Order(id, userName, book, orderedOn, arrivesOn);
                orders.Add(order);
            }
        }

        return orders;
    }
}