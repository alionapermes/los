using Npgsql;

using los.Models;
using Microsoft.AspNetCore.Mvc;

namespace los.Util;

public static class Data
{
  public static List<Order> GetAllOrders(NpgsqlDataSource dataSource)
  {
    var orders = new List<Order>();

    var sql =
      "SELECT " +
        "o.id, o.username, b.id as book_id, b.title as book_title, " +
        "o.ordered_on, o.arrives_on " +
      "FROM \"order\" AS o LEFT JOIN book AS b ON o.book_id = b.id";

    using (var cmd = dataSource.CreateCommand(sql))
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

  public static Order GetOrderByID(NpgsqlDataSource dataSource, Int64 orderID)
  {
    var sql =
      "SELECT " +
        "username, book_id, ordered_on, arrives_on " +
      "FROM \"order\" WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", orderID);
      using (var reader = cmd.ExecuteReader()) {
        if (reader.Read()) {
          var userName = reader.GetString(0);
          var bookID = reader.GetInt64(1);
          var orderedOn = reader.GetDateTime(2);
          var arrivesOn = reader.GetDateTime(3);

          var book = new Book(bookID, string.Empty);

          return new Order(orderID, userName, book, orderedOn, arrivesOn);
        }
      }
    }

    return null;
  }

  public static void UpdateOrder(NpgsqlDataSource dataSource, Order order)
  {
    var sql =
      "UPDATE \"order\" SET " +
        "username = @username," +
        "book_id = @book_id," +
        "ordered_on = @ordered_on," +
        "arrives_on = @arrives_on " +
      "WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", order.ID);
      cmd.Parameters.AddWithValue("username", order.UserName);
      cmd.Parameters.AddWithValue("book_id", order.Book.ID);
      cmd.Parameters.AddWithValue("ordered_on", order.OrderedOn);
      cmd.Parameters.AddWithValue("arrives_on", order.ArrivesOn);
      cmd.ExecuteNonQuery();
    }
  }

  public static void AddOrder(NpgsqlDataSource dataSource, Order order)
  {
    var sql =
      "INSERT INTO \"order\" " +
        "(book_id, username, ordered_on, arrives_on) VALUES" +
        "(@book_id, @username, @ordered_on, @arrives_on)";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("book_id", order.Book.ID);
      cmd.Parameters.AddWithValue("username", order.UserName);
      cmd.Parameters.AddWithValue("ordered_on", order.OrderedOn);
      cmd.Parameters.AddWithValue("arrives_on", order.ArrivesOn);
      cmd.ExecuteNonQuery();
    }
  }

  public static List<Book> GetAllBooks(NpgsqlDataSource dataSource)
  {
    var books = new List<Book>();

    var sql = "SELECT id, title FROM book";

    using (var cmd = dataSource.CreateCommand(sql))
    using (var reader = cmd.ExecuteReader()) {
      while (reader.Read()) {
        var id = reader.GetInt64(0);
        var title = reader.GetString(1);
        books.Add(new Book(id, title));
      }
    }

    return books;
  }

  public static Book GetBookByID(NpgsqlDataSource dataSource, Int64 bookID)
  {
    var sql = "SELECT title FROM book WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", bookID);
      using (var reader = cmd.ExecuteReader()) {
        if (reader.Read()) {
          return new Book(bookID, reader.GetString(0));
        }
      }
    }

    return null;
  }

  public static void UpdateBook(NpgsqlDataSource dataSource, Book book)
  {
    var sql = "UPDATE book SET title = @title WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", book.ID);
      cmd.Parameters.AddWithValue("title", book.Title);
      cmd.ExecuteNonQuery();
    }
  }
}