using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Data;

namespace los.Pages;

[BindProperties]
public class AdminOrdersModel : PageModel
{
    public string DateFormat{get;} = "dd.MM.yyyy";

    public string? ActionType{get; set;}
    public string? ErrMsg{get; set;}
    public Int64? InputID{get; set;}
    public Int64? InputUserID{get; set;}
    public Int64? InputBookID{get; set;}
    public string? InputSecretCode{get; set;}
    public string? InputOrderedOn{get; set;}
    public string? InputArrivesOn{get; set;}

    private NpgsqlDataSource _dataSource;

    public AdminOrdersModel()
    {
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public void OnPost()
    {
        Console.WriteLine("AdminOrdersModel.ActionType: " + ActionType);
        switch (ActionType) {
            case "insert": this.addOrder(); break;
            case "remove": this.removeOrder(); break;
            default: return;
        }
    }

    public List<Order> GetAllOrders()
    {
        return OrdersData.GetAll(_dataSource);
    }

    public List<User> GetAllUsers()
    {
        return UsersData.GetAll(_dataSource);
    }

    public List<Book> GetAllBooks()
    {
        return BooksData.GetAll(_dataSource);
    }

    public void addOrder()
    {
        User user;
        Book book;

        Int64 bookID = InputBookID ?? 0;
        if (bookID < 0) {
            Console.WriteLine("addOrder: incorrect book id");
            return;
        }

        Int64 userID = InputUserID ?? 0;
        if (userID < 0) {
            Console.WriteLine("addOrder: incorrect user id");
            return;
        }

        if (string.IsNullOrEmpty(InputSecretCode)) {
            Console.WriteLine("addOrder: incorrect secret code");
            return;
        }

        DateTime orderedOn;
        try {
            orderedOn = string.IsNullOrEmpty(InputOrderedOn)
                ? DateTime.Today
                : DateTime.ParseExact(InputOrderedOn, DateFormat, null);            
        } catch (Exception e) {
            Console.WriteLine("addOrder: incorrect date entered");
            return;
        }

        DateTime arrivesOn;
        try {
            arrivesOn = DateTime.ParseExact(InputArrivesOn, DateFormat, null);
        } catch (Exception e) {
            Console.WriteLine("addOrder: incorrect date entered");
            return;
        }

        if (arrivesOn <= orderedOn) {
            Console.WriteLine("addOrder: incorrect date entered");
            ErrMsg = "arriving date cannot be earlier or equal than Today";
            return;
        }

        book = BooksData.GetByID(_dataSource, bookID);
        user = UsersData.GetByID(_dataSource, userID);
        var order = new Order(InputSecretCode, user, book, orderedOn, arrivesOn);

        OrdersData.Add(_dataSource, order);
    }

    public void removeOrder()
    {
        Int64 orderID = InputID ?? 0;
        if (orderID <= 0) {
            Console.WriteLine("removeOrder: incorrect order id");
            return;
        }

        OrdersData.DeleteByID(_dataSource, orderID);
    }
}