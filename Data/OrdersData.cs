using Microsoft.AspNetCore.Mvc;

using Npgsql;

using los.Models;

namespace los.Data;

public static class OrdersData
{
  public static List<Order> GetAll(NpgsqlDataSource dataSource)
  {
    var orders = new List<Order>();

    var sql =
      "SELECT " +
        //          0                  1
        "u.id AS user_id, u.name AS user_name, " +
        //          2                        3
        "a.id AS author_id, a.fullname AS author_name, " +
        //          4                    5
        "g.id AS genre_id, g.title AS genre_title, " +
        //          6                    7                     8
        "b.id AS book_id, b.year AS book_year, b.title AS book_title, " +
        // 9        10            11            12
        "o.id, o.secret_code, o.ordered_on, o.arrives_on " +
      "FROM \"order\" AS o " +
      "LEFT JOIN \"user\" AS u ON o.user_id   = u.id " +
      "LEFT JOIN book     AS b ON o.book_id   = b.id " +
      "LEFT JOIN author   AS a ON b.author_id = a.id " +
      "LEFT JOIN genre    AS g ON b.genre_id  = g.id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      using (var reader = cmd.ExecuteReader()) {
        while (reader.Read()) {
          var userID = reader.GetInt64(0);
          var userName = reader.GetString(1);
          var user = new User(userID, userName);

          var authorID = reader.GetInt64(2);
          var authorName = reader.GetString(3);
          var author = new Author(authorID, authorName);

          var genreID = reader.GetInt64(4);
          var genreTitle = reader.GetString(5);
          var genre = new Genre(genreID, genreTitle);

          var bookID = reader.GetInt64(6);
          var bookYear = reader.GetInt32(7);
          var bookTitle = reader.GetString(8);
          var book = new Book(bookID, bookTitle, bookYear, author, genre);

          var orderID = reader.GetInt64(9);
          var secretCode = reader.GetString(10);
          var orderedOn = reader.GetDateTime(11);
          var arrivesOn = reader.GetDateTime(12);
          var order = new Order(orderID, secretCode, user, book, orderedOn, arrivesOn);

          orders.Add(order);
        }
      }
    }

    return orders;
  }

  public static List<Order> GetAllByUserID(NpgsqlDataSource dataSource, Int64 userID)
  {
    var orders = new List<Order>();

    var sql =
      "SELECT " +
        //          0
        "u.name AS user_name, " +
        //          1                        2
        "a.id AS author_id, a.fullname AS author_name, " +
        //          3                    4
        "g.id AS genre_id, g.title AS genre_title, " +
        //          5                    6                     7
        "b.id AS book_id, b.year AS book_year, b.title AS book_title, " +
        // 8        9              10            11
        "o.id, o.secret_code, o.ordered_on, o.arrives_on " +
      "FROM \"order\" AS o " +
      "LEFT JOIN \"user\" AS u ON o.user_id   = u.id " +
      "LEFT JOIN book     AS b ON o.book_id   = b.id " +
      "LEFT JOIN author   AS a ON b.author_id = a.id " +
      "LEFT JOIN genre    AS g ON b.genre_id  = g.id " +
      "WHERE o.user_id = @user_id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("user_id", userID);
      using (var reader = cmd.ExecuteReader()) {
        while (reader.Read()) {
          var userName = reader.GetString(0);
          var user = new User(userID, userName);

          var authorID = reader.GetInt64(1);
          var authorName = reader.GetString(2);
          var author = new Author(authorID, authorName);

          var genreID = reader.GetInt64(3);
          var genreTitle = reader.GetString(4);
          var genre = new Genre(genreID, genreTitle);

          var bookID = reader.GetInt64(5);
          var bookYear = reader.GetInt32(6);
          var bookTitle = reader.GetString(7);
          var book = new Book(bookID, bookTitle, bookYear, author, genre);

          var orderID = reader.GetInt64(8);
          var secretCode = reader.GetString(9);
          var orderedOn = reader.GetDateTime(10);
          var arrivesOn = reader.GetDateTime(11);
          var order = new Order(orderID, secretCode, user, book, orderedOn, arrivesOn);

          orders.Add(order);
        }
      }
    }

    return orders;
  }

  public static Order GetByID(NpgsqlDataSource dataSource, Int64 id)
  {
    var sql =
      "SELECT " +
        //          0                  1
        "u.id AS user_id, u.name AS user_name, " +
        //          2                        3
        "a.id AS author_id, a.fullname AS author_name, " +
        //          4                    5
        "g.id AS genre_id, g.title AS genre_title, " +
        //          6                    7                     8
        "b.id AS book_id, b.year AS book_year, b.title AS book_title, " +
        //     9            10            11
        "o.secret_code, o.ordered_on, o.arrives_on " +
      "FROM \"order\" AS o " +
      "LEFT JOIN \"user\" AS u ON o.user_id   = u.id " +
      "LEFT JOIN book     AS b ON o.book_id   = b.id " +
      "LEFT JOIN author   AS a ON b.author_id = a.id " +
      "LEFT JOIN genre    AS g ON b.genre_id  = g.id " +
      "WHERE o.id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", id);
      using (var reader = cmd.ExecuteReader()) {
        if (reader.Read()) {
          var userID = reader.GetInt64(0);
          var userName = reader.GetString(1);
          var user = new User(userID, userName);

          var authorID = reader.GetInt64(2);
          var authorName = reader.GetString(3);
          var author = new Author(authorID, authorName);

          var genreID = reader.GetInt64(4);
          var genreTitle = reader.GetString(5);
          var genre = new Genre(genreID, genreTitle);

          var bookID = reader.GetInt64(6);
          var bookYear = reader.GetInt32(7);
          var bookTitle = reader.GetString(8);
          var book = new Book(bookID, bookTitle, bookYear, author, genre);

          var secretCode = reader.GetString(9);
          var orderedOn = reader.GetDateTime(10);
          var arrivesOn = reader.GetDateTime(11);
          var order = new Order(id, secretCode, user, book, orderedOn, arrivesOn);

          return order;
        }
      }
    }

    return null;
  }

  public static void Update(NpgsqlDataSource dataSource, Order order)
  {
    var sql =
      "UPDATE \"order\" SET " +
        "user_id = @user_id," +
        "book_id = @book_id," +
        "secret_code = @secret_code," +
        "ordered_on = @ordered_on," +
        "arrives_on = @arrives_on " +
      "WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", order.ID);
      cmd.Parameters.AddWithValue("user_id", order.User.ID);
      cmd.Parameters.AddWithValue("book_id", order.Book.ID);
      cmd.Parameters.AddWithValue("secret_code", order.SecretCode);
      cmd.Parameters.AddWithValue("ordered_on", order.OrderedOn);
      cmd.Parameters.AddWithValue("arrives_on", order.ArrivesOn);
      cmd.ExecuteNonQuery();
    }
  }

  public static Int64 Add(NpgsqlDataSource dataSource, Order order)
  {
    var sql =
      "INSERT INTO \"order\" " +
        "(user_id, book_id, secret_code, ordered_on, arrives_on) VALUES" +
        "(@user_id, @book_id, @secret_code, @ordered_on, @arrives_on) " +
      "RETURNING id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("user_id", order.User.ID);
      cmd.Parameters.AddWithValue("book_id", order.Book.ID);
      cmd.Parameters.AddWithValue("secret_code", order.SecretCode);
      cmd.Parameters.AddWithValue("ordered_on", order.OrderedOn);
      cmd.Parameters.AddWithValue("arrives_on", order.ArrivesOn);
      
      var id = cmd.ExecuteScalar();
      return Convert.ToInt64(id);
    }
  }

  public static void DeleteByID(NpgsqlDataSource dataSource, Int64 id)
  {
    var sql = "DELETE FROM \"order\" WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", id);
      cmd.ExecuteNonQuery();
    }
  }
}