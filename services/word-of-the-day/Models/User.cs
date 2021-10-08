using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public DateTime LastUpdated { get; set; }
    public int WordOfTheDayId { get; set; }
}
