using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;
using System.Security.Cryptography.X509Certificates;

namespace los.Pages;

[BindProperties]
public class AdminEditOrderModel : PageModel
{
    public string DateFormat{get;} = "dd.MM.yyyy";

    public string? ErrMsg{get; set;}
    public Int64? InputID{get; set;}
    public Int64? InputUserID{get; set;}
    public Int64? InputBookID{get; set;}
    public string? InputSecretCode{get; set;}
    public string? InputOrderedOn{get; set;}
    public string? InputArrivesOn{get; set;}

    private NpgsqlDataSource _dataSource;

    public AdminEditOrderModel()
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
        InputUserID = order.User.ID;
        InputBookID = order.Book.ID;
        InputSecretCode = order.SecretCode;
        InputOrderedOn = order.OrderedOn.ToString(DateFormat);
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

        Int64 bookID = InputBookID ?? 0;
        if (bookID <= 0) {
            Console.WriteLine("AdminEditOrder.OnPost: incorrect book id");
            return new EmptyResult();
        }

        Int64 userID = InputUserID ?? 0;
        if (userID <= 0) {
            Console.WriteLine("AdminEditOrder.OnPost: incorrect user id");
            return new EmptyResult();
        }

        if (string.IsNullOrEmpty(InputSecretCode)) {
            Console.WriteLine("AdminEditOrder.OnPost: empty secret code");
            return new EmptyResult();
        }

        DateTime orderedOn;
        try {
            orderedOn = string.IsNullOrEmpty(InputOrderedOn)
                ? DateTime.Today
                : DateTime.ParseExact(InputOrderedOn, DateFormat, null);            
        } catch (Exception e) {
            Console.WriteLine("addOrder: incorrect date entered");
            return new EmptyResult();
        }

        DateTime arrivesOn;
        try {
            arrivesOn = string.IsNullOrEmpty(InputArrivesOn)
                ? orderedOn.AddDays(1)
                : DateTime.ParseExact(InputArrivesOn, DateFormat, null);
        } catch (Exception e) {
            Console.WriteLine("addOrder: incorrect date entered");
            return new EmptyResult();
        }

        if (arrivesOn <= orderedOn) {
            Console.WriteLine("addOrder: incorrect date entered");
            ErrMsg = "arriving date cannot be earlier or equal than Today";
            return new EmptyResult();
        }

        order.Book = BooksData.GetByID(_dataSource, bookID);
        order.User = UsersData.GetByID(_dataSource, userID);
        order.SecretCode = InputSecretCode;
        order.OrderedOn = orderedOn;
        order.ArrivesOn = arrivesOn;
        OrdersData.Update(_dataSource, order);

        return Redirect("/AdminOrders");
    }

    public List<Book> GetAllBooks()
    {
        return BooksData.GetAll(_dataSource);
    }

    public List<User> GetAllUsers()
    {
        return UsersData.GetAll(_dataSource);
    }
}