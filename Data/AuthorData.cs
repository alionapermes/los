using Microsoft.AspNetCore.Mvc;

using Npgsql;

using los.Models;

namespace los.Data;

public class AuthorsData
{
  public static List<Author> GetAll(NpgsqlDataSource dataSource)
  {
    var authors = new List<Author>();
    var sql = "SELECT id, fullname FROM author";

    using (var cmd = dataSource.CreateCommand(sql))
    using (var reader = cmd.ExecuteReader()) {
      while (reader.Read()) {
        var id = reader.GetInt64(0);
        var title = reader.GetString(1);

        authors.Add(new Author(id, title));
      }
    }

    return authors;
  }

  public static Author GetByID(NpgsqlDataSource dataSource, Int64 id)
  {
    var sql = "SELECT fullname FROM \"author\" WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", id);
      using (var reader = cmd.ExecuteReader()) {
        if (reader.Read()) {
          return new Author(id, reader.GetString(0));
        }
      }
    }

    return null;
  }

  public static void Update(NpgsqlDataSource dataSource, Author author)
  {
    var sql = "UPDATE \"author\" SET fullname = @fullname WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", author.ID);
      cmd.Parameters.AddWithValue("fullname", author.Fullname);
      cmd.ExecuteNonQuery();
    }
  }

  public static void Add(NpgsqlDataSource dataSource, Author author)
  {
    var sql = "INSERT INTO \"author\" (title) VALUES(@fullname)";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("fullname", author.Fullname);
      cmd.ExecuteNonQuery();
    }
  }

  public static void DeleteByID(NpgsqlDataSource dataSource, Int64 id)
  {
    var sql = "DELETE FROM \"author\" WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", id);
      cmd.ExecuteNonQuery();
    }
  }
}