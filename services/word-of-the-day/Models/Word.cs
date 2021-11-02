using System.ComponentModel.DataAnnotations;

public class Word
{
    [Key]
    public int WordId { get; set; }
    public string Text { get; set; }
    public string Type { get; set; }
    public string Definition { get; set; }
}
