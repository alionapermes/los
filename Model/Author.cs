namespace los.Models;

public class Author
{
  public Int64 ID{get; set;}
  public string Fullname{get; set;}

  public Author(string fullname)
  {
    this.Fullname = fullname;
  }

  public Author(Int64 id, string fullname) : this(fullname)
  {
    this.ID = id;
  }
}