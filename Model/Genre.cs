namespace los.Models;

public class Genre
{
  public Int64 ID{get; set;}
  public string Title{get; set;}

  public Genre(string title)
  {
    this.Title = title;
  }

  public Genre(Int64 id, string title) : this(title)
  {
    this.ID = id;
  }
}