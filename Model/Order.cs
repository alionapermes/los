namespace los.Models;

public class Order
{
  public Int64 ID{get; set;}
  public string SecretCode{get; set;}
  public User User{get; set;}
  public Book Book{get; set;}
  public DateTime OrderedOn{get; set;} = new();
  public DateTime ArrivesOn{get; set;} = new();

  public Order(
    string secretCode,
    User user,
    Book book,
    DateTime orderedOn,
    DateTime arrivesOn
  ) {
    this.SecretCode = secretCode;
    this.User = user;
    this.Book = book;
    this.OrderedOn = orderedOn;
    this.ArrivesOn = arrivesOn;

    this.SecretCode = Guid.NewGuid().ToString();
  }

  public Order(
    Int64 id,
    string secretCode,
    User user,
    Book book,
    DateTime orderedOn,
    DateTime arrivedOn
  ) : this(secretCode, user, book, orderedOn, arrivedOn) {
    this.ID = id;
  }

  public Order(Int64 id)
  {
    this.ID = id;
  }
}