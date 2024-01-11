namespace los.Models;

public class User
{
  public Int64 ID{get; set;}
  public Int32 Year{get; set;}
  public string Name{get; set;}
  public string Authors{get; set;}
  public Genre Genre{get; set;}

  public User(string name)
  {
    this.Name = name;
  }

  public User(Int64 id, string name) : this(name)
  {
    this.ID = id;
  }
}