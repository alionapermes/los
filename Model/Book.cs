namespace los.Models;

public class Book
{
  public Int64 ID{get; set;}
  public Int32 Year{get; set;}
  public string Title{get; set;}
  public Author Author{get; set;}
  public Genre? Genre{get; set;} = null;

  public Book(string title, Int32 year, Author author, Genre? genre = null)
  {
    this.Title = title;
    this.Year = year;
    this.Author = author;
    this.Genre = genre;
  }

  public Book(Int64 id, string title, Int32 year, Author author, Genre? genre = null)
    : this(title, year, author, genre)
  {
    this.ID = id;
  }
}