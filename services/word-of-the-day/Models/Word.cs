using System.ComponentModel.DataAnnotations;

public class Word
{
    [Key]
    public int WordId { get; set; }
    public string WordText { get; set; }
    public string Definition { get; set; }
}
