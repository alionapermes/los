using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class UserEditOrderModel : PageModel
{
    public string DateFormat{get;} = "dd.MM.yyyy";

    public string? ErrMsg{get; set;}
    public Int64? InputID{get; set;}
    public Int64? InputBookID{get; set;}
    public string? InputArrivesOn{get; set;}

    public string? InputUserName{get; set;}
    public string? InputSecretCode{get; set;}

    private NpgsqlDataSource _dataSource;

    public UserEditOrderModel()
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

        var order = OrdersData.GetByID(_dataSource, id);

        InputID = id;
        InputUserName = order.User.Name;
        InputBookID = order.Book.ID;
        InputArrivesOn = order.ArrivesOn.ToString(DateFormat);
    }

    public IActionResult OnPost()
    {
        Int64 orderID = InputID ?? 0;
        if (orderID <= 0) {
            Console.WriteLine("AdminEditOrder.OnPost: incorrect id");
            return new EmptyResult();
        }

        var order = OrdersData.GetByID(_dataSource, orderID);

        DateTime arrivesOn;
        try {
            arrivesOn = string.IsNullOrEmpty(InputArrivesOn)
                ? DateTime.Today.AddDays(1)
                : DateTime.ParseExact(InputArrivesOn, DateFormat, null);
        } catch (Exception e) {
            Console.WriteLine("addOrder: incorrect date entered");
            return new EmptyResult();
        }

        order.ArrivesOn = arrivesOn;
        OrdersData.Update(_dataSource, order);

        return Redirect("/AdminOrders");
    }

    public List<Book> GetAllBooks()
    {
        return BooksData.GetAll(_dataSource);
    }
}