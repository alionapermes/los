namespace los.Models;

public class Book
{
  public Int64 ID{get; set;}
  public string Title{get; set;}

  public Book(string title)
  {
    this.Title = title;
  }

  public Book(Int64 id, string title) : this(title)
  {
    this.ID = id;
  }
}