using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public DateTime LastUpdated { get; set; }
    public int WordOfTheDayId { get; set; }
}
