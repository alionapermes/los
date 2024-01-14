using Microsoft.AspNetCore.Mvc;

using Npgsql;

using los.Models;

namespace los.Data;

public class UsersData
{
  public static List<User> GetAll(NpgsqlDataSource dataSource)
  {
    var users = new List<User>();
    var sql = "SELECT id, name FROM \"user\"";

    using (var cmd = dataSource.CreateCommand(sql))
    using (var reader = cmd.ExecuteReader()) {
      while (reader.Read()) {
        var id = reader.GetInt64(0);
        var title = reader.GetString(1);

        users.Add(new User(id, title));
      }
    }

    return users;
  }

  public static User GetByID(NpgsqlDataSource dataSource, Int64 id)
  {
    var sql = "SELECT name FROM \"user\" WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", id);
      using (var reader = cmd.ExecuteReader()) {
        if (reader.Read()) {
          return new User(id, reader.GetString(0));
        }
      }
    }

    return null;
  }

  public static User FindByName(NpgsqlDataSource dataSource, string name)
  {
    var sql = "SELECT id FROM \"user\" WHERE name = @name";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("name", name);
      using (var reader = cmd.ExecuteReader()) {
        if (reader.Read()) {
          return new User(reader.GetInt64(0), name);
        }
      }
    }

    return null;
  }

  public static void Update(NpgsqlDataSource dataSource, User user)
  {
    var sql = "UPDATE \"user\" SET name = @name WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", user.ID);
      cmd.Parameters.AddWithValue("name", user.Name);
      cmd.ExecuteNonQuery();
    }
  }

  public static Int64 Add(NpgsqlDataSource dataSource, User user)
  {
    var sql = "INSERT INTO \"user\" (name) VALUES(@name) RETURNING id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("name", user.Name);
      var id = cmd.ExecuteScalar();
      return Convert.ToInt64(id);
    }
  }

  public static void DeleteByID(NpgsqlDataSource dataSource, Int64 id)
  {
    var sql = "DELETE FROM \"user\" WHERE id = @id";

    using (var cmd = dataSource.CreateCommand(sql)) {
      cmd.Parameters.AddWithValue("id", id);
      cmd.ExecuteNonQuery();
    }
  }
}