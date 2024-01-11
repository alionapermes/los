using Microsoft.AspNetCore.Mvc;

using Npgsql;

using los.Models;

namespace los.Data;

public class GenresData
{
  public static List<Genre> GetAll(NpgsqlDataSource dataSource)
  {
    var genres = new List<Genre>();
    var sql = "SELECT id, title FROM genre";

    using (var cmd = dataSource.CreateCommand(sql))
    using (var reader = cmd.ExecuteReader()) {
      while (reader.Read()) {
        var id = reader.GetInt64(0);
        var title = reader.GetString(1);

        genres.Add(new Genre(id, title));
      }
    }

    return genres;
  }

  public static Genre GetByID(NpgsqlDataSource dataSource, Int64 id)
  {
    var sql = "SELECT title FROM genre WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", id);
      using (var reader = cmd.ExecuteReader()) {
        if (reader.Read()) {
          return new Genre(id, reader.GetString(0));
        }
      }
    }

    return null;
  }

  public static void Update(NpgsqlDataSource dataSource, Genre genre)
  {
    var sql = "UPDATE genre SET title = @title, WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", genre.ID);
      cmd.Parameters.AddWithValue("title", genre.Title);
      cmd.ExecuteNonQuery();
    }
  }

  public static void Add(NpgsqlDataSource dataSource, Genre genre)
  {
    var sql = "INSERT INTO genre (title) VALUES(@title)";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("title", genre.Title);
      cmd.ExecuteNonQuery();
    }
  }

  public static void DeleteByID(NpgsqlDataSource dataSource, Int64 id)
  {
    var sql = "DELETE FROM genre WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", id);
      cmd.ExecuteNonQuery();
    }
  }
}