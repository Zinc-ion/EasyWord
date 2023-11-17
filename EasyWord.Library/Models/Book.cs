namespace EasyWord.Library.Models;

[SQLite.Table("Books")]
public class Book
{
    [SQLite.Column("Id")]
    public int id { get; set; }

    [SQLite.Column("Name")]
    public string name { get; set; }
}