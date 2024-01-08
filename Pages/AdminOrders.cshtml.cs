using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Npgsql;

using los.Models;
using los.Util;

namespace los.Pages;

[BindProperties]
public class AdminOrdersModel : PageModel
{
    public string DateFormat{get;} = "dd.MM.yyyy";

    public string? ActionType{get; set;}
    public string? ErrMsg{get; set;}
    public Int64? InputID{get; set;}
    public string? InputUserName{get; set;}
    public Int64? InputBookID{get; set;}
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

    public List<Book> GetAllBooks()
    {
        return Data.GetAllBooks(_dataSource);
    }

    public List<Order> GetAllOrders()
    {
        return Data.GetAllOrders(_dataSource);
    }

    public void addOrder()
    {
        Int64 bookID = InputBookID ?? 0;
        if (bookID < 0) {
            Console.WriteLine("addOrder: incorrect book choosen");
            return;
        }

        if (string.IsNullOrEmpty(InputUserName)) {
            Console.WriteLine("addOrder: incorrect username");
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

        var book = new Book(bookID, string.Empty);
        var order = new Order(InputUserName, book, orderedOn, arrivesOn);

        Data.AddOrder(_dataSource, order);
    }

    public void removeOrder()
    {
        Int64 orderID = InputID ?? 0;
        if (orderID <= 0) {
            Console.WriteLine("removeOrder: incorrect order id");
            return;
        }

        var sql = "DELETE FROM \"order\" WHERE id = @id";

        using (var cmd = _dataSource.CreateCommand(sql)) {
            cmd.Parameters.AddWithValue("id", orderID);
            cmd.ExecuteNonQuery();
        }
    }
}