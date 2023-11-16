namespace EasyWord.Library.Models;

[SQLite.Table("CET4_1")]
public class Word
{
    [SQLite.Column("wordRank")]
    public int wordRank { get; set; }

    [SQLite.Column("bookId")]
    public string bookId { get; set; }

    [SQLite.Column("status")]
    public int status { get; set; }

    [SQLite.Column("headWord")]
    public string headWord { get; set; }

    [SQLite.Column("tranCN")]
    public string tranCN { get; set; }

    [SQLite.Column("pos")]
    public string pos { get; set; }

    [SQLite.Column("daysBetweenReviews")]
    public float daysBetweenReviews { get; set; }

    [SQLite.Column("lastScore")]
    public float lastScore { get; set; }

    [SQLite.Column("dateLastReviewed")]
    public string dateLastReviewed { get; set; }

}