using Microsoft.AspNetCore.Mvc;

using Npgsql;

using los.Models;

namespace los.Data;

public class BooksData
{
  public static List<Book> GetAll(NpgsqlDataSource dataSource)
  {
    var books = new List<Book>();
    var sql =
      "SELECT " +
        //          0                        1
        "a.id AS author_id, a.fullname AS author_name, " +
        //          2                        3
        "g.id AS genre_id, g.title AS genre_title " +
        // 4     5        6
        "b.id, b.title, b.year, " +
      "FROM book AS b " +
      "LEFT JOIN author AS a ON b.author_id = a.id " +
      "LEFT JOIN genre AS g ON b.genre_id = g.id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      using (var reader = cmd.ExecuteReader()) {
        if (reader.Read()) {
          var authorID = reader.GetInt64(0);
          var authorName = reader.GetString(1);
          var author = new Author(authorID, authorName);

          var genreID = reader.GetInt64(2);
          var genreTitle = reader.GetString(3);
          var genre = new Genre(genreID, genreTitle);

          var bookID = reader.GetInt64(4);
          var title = reader.GetString(5);
          var year = reader.GetInt32(6);
          var book = new Book(bookID, title, year, author, genre);

          books.Add(book);
        }
      }
    }

    return books;
  }

  public static Book GetByID(NpgsqlDataSource dataSource, Int64 id)
  {
    var sql =
      "SELECT " +
        //          0                        1
        "a.id AS author_id, a.fullname AS author_name, " +
        //          2                        3
        "g.id AS genre_id, g.title AS genre_title " +
        // 4        5
        "b.title, b.year, " +
      "FROM book AS b " +
      "LEFT JOIN author AS a ON b.author_id = a.id " +
      "LEFT JOIN genre AS g ON b.genre_id = g.id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", id);
      using (var reader = cmd.ExecuteReader()) {
        if (reader.Read()) {
          var authorID = reader.GetInt64(0);
          var authorName = reader.GetString(1);
          var author = new Author(authorID, authorName);

          var genreID = reader.GetInt64(2);
          var genreTitle = reader.GetString(3);
          var genre = new Genre(genreID, genreTitle);

          var title = reader.GetString(4);
          var year = reader.GetInt32(5);
          var book = new Book(id, title, year, author, genre);

          return book;
        }
      }
    }

    return null;
  }

  public static void Update(NpgsqlDataSource dataSource, Book book)
  {
    var sql =
      "UPDATE book SET " +
        "genre_id = @genre_id," +
        "author_id = @author_id," +
        "title = @title," +
        "year = @year " +
      "WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", book.ID);
      cmd.Parameters.AddWithValue("genre_id", book.Genre.ID);
      cmd.Parameters.AddWithValue("author_id", book.Author.ID);
      cmd.Parameters.AddWithValue("title", book.Title);
      cmd.Parameters.AddWithValue("year", book.Year);
      cmd.ExecuteNonQuery();
    }
  }

  public static void Add(NpgsqlDataSource dataSource, Book book)
  {
    var sql =
      "INSERT INTO book " +
        "(genre_id, author_id, title, year) VALUES" +
        "(@genre_id, @author_id @title, @year)";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("genre_id", book.Genre.ID);
      cmd.Parameters.AddWithValue("author_id", book.Author);
      cmd.Parameters.AddWithValue("title", book.Title);
      cmd.Parameters.AddWithValue("year", book.Year);
      cmd.ExecuteNonQuery();
    }
  }

  public static void DeleteByID(NpgsqlDataSource dataSource, Int64 id)
  {
    var sql = "DELETE FROM book WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", id);
      cmd.ExecuteNonQuery();
    }
  }
}