using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class UserOrdersModel : PageModel
{
    public string DateFormat{get;} = "dd.MM.yyyy";

    public string? ErrMsg{get; set;}
    public string? ActionType{get; set;}
    public Int64? InputBookID{get; set;}
    public string? InputArrivesOn{get; set;}
    public string? InputSecretCode{get; set;}
    public Int64? InputUserID{get; set;}
    public Int64? InputOrderID{get; set;}
    private NpgsqlDataSource _dataSource;

    public UserOrdersModel()
    {
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public IActionResult OnGet()
    {
        Int64 userID = 0;
        if (!Int64.TryParse(Request.Query["user_id"], out userID)) {
            Console.WriteLine("cannot to parse user_id: " + Request.QueryString);
            return Redirect("/UserInterface");
        }

        InputUserID = userID;
        Console.WriteLine("userID: " + InputUserID);

        return Page();
    }

    public IActionResult OnPost()
    {
        Console.WriteLine("UserOrdersModel.ActionType: " + this.ActionType);
        switch (this.ActionType) {
            case "insert":
                Order? order = this.addOrder();
                if (order == null) {
                    return Redirect("/UserInterface");
                }
                string queryParams = $"id={order.ID}&secret_code={order.SecretCode}";
                return Redirect($"/UserOrderSuccess?{queryParams}");
            case "cancel":
                if (string.IsNullOrEmpty(InputSecretCode)) {
                    ErrMsg = "empty secret code";
                    return Page();
                }
                this.cancelOrder();
                return Redirect("/UserInterface");
            default: return Redirect("/Index");
        }
    }

    public void cancelOrder()
    {
        Int64 orderID = InputOrderID ?? 0;
        if (orderID <= 0) {
            Console.WriteLine("cancelOrder: incorrect orderID: " + orderID);
            return;
        }

        string secretCode = InputSecretCode ?? string.Empty;
        if (secretCode == string.Empty) {
            Console.WriteLine("cancelOrder: empty secret code");
            return;
        }

        var order = OrdersData.GetByID(_dataSource, orderID);
        if (order.SecretCode != secretCode) {
            ErrMsg = "incorrect sercret code";
            Console.WriteLine("cancelOrder: " + ErrMsg);
            return;
        }

        OrdersData.DeleteByID(_dataSource, orderID);
    }

    public Order? addOrder()
    {
        Int64 userID = InputUserID ?? 0;
        if (userID <= 0) {
            Console.WriteLine("addOrder: incorrect userID: " + userID);
            return null;
        }

        Int64 bookID = InputBookID ?? 0;
        if (bookID <= 0) {
            Console.WriteLine("addOrder: incorrect book id: " + bookID);
            return null;
        }

        var orderedOn = DateTime.Today;

        DateTime arrivesOn;
        try {
            arrivesOn = DateTime.ParseExact(InputArrivesOn, DateFormat, null);
        } catch (Exception e) {
            Console.WriteLine("addOrder: incorrect date entered");
            return null;
        }

        if (arrivesOn <= orderedOn) {
            Console.WriteLine("addOrder: incorrect date entered");
            this.ErrMsg = "arriving date cannot be earlier or equal than Today";
            return null;
        }

        var book = BooksData.GetByID(_dataSource, bookID);
        var user = UsersData.GetByID(_dataSource, userID);
        var order = new Order(user, book, orderedOn, arrivesOn);

        Int64 orderID = OrdersData.Add(_dataSource, order);
        order.ID = orderID;

        return order;
    }

    public List<Order> GetMyOrders()
    {
        Int64 userID = InputUserID ?? 0;
        if (userID <= 0) {
            this.ErrMsg = "incorrect user_id";
            return new List<Order>();
        }
        return OrdersData.GetAllByUserID(_dataSource, userID);
    }

    public List<Book> GetAllBooks()
    {
        return BooksData.GetAll(_dataSource);
    }
}