using System;
using System.ComponentModel.DataAnnotations;

public class PreviouslyUsedWord
{
    [Key]
    public int Id { get; set; }
    public int WordId { get; set; }
    public string UserId { get; set; }
}
