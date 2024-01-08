namespace los.Models;

public class Order
{
  public Int64 ID{get; set;}
  public string UserName{get; set;}
  public Book Book{get; set;}
  public DateTime OrderedOn{get; set;} = new();
  public DateTime ArrivesOn{get; set;} = new();

  public Order(
    string userName,
    Book book,
    DateTime orderedOn,
    DateTime arrivesOn
  ) {
    this.UserName = userName;
    this.Book = book;
    this.OrderedOn = orderedOn;
    this.ArrivesOn = arrivesOn;
  }

  public Order(
    Int64 id,
    string userName,
    Book book,
    DateTime orderedOn,
    DateTime arrivedOn
  ) : this(userName, book, orderedOn, arrivedOn) {
    this.ID = id;
  }
}