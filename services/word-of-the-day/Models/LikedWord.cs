using System.ComponentModel.DataAnnotations;

public class LikedWord
{
    [Key]
    public int Id { get; set; }
    public int WordId { get; set; }
    public string Username { get; set; }
}
