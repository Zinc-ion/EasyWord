using SQLite;

namespace EasyWord.Library.Models;

[SQLite.Table("CET4_1")]
public class Word
{
    [PrimaryKey]
    [SQLite.Column("wordRank")]
    public int WordRank { get; set; }

    [SQLite.Column("bookId")]
    public string BookId { get; set; }

    [SQLite.Column("status")]
    public int Status { get; set; }

    [SQLite.Column("headWord")]
    public string HeadWord { get; set; }

    [SQLite.Column("usphone")]
    public string Usphone { get; set; }

    [SQLite.Column("ukphone")]
    public string Ukphone { get; set; }

    [SQLite.Column("tranCN")]
    public string TranCN { get; set; }

    [SQLite.Column("sentenceCN")]
    public string SentenceCN { get; set; }

    [SQLite.Column("sentence")]
    public string Sentence { get; set; }

    [SQLite.Column("pos")]
    public string Pos { get; set; }

    [SQLite.Column("dateLastReviewed")]
    public string DateLastReviewed { get; set; }

    [SQLite.Column("dateNextReview")]
    public string DateNextReview { get; set; }

    [SQLite.Column("dateRecite")]
    public string DateRecite { get; set; }
}