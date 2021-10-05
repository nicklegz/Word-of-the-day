using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public Guid UserId { get; set; }
    public string Username { get; set; }

    //used to reference the primary key in the Word table
    public List<int> PreviouslyUsedWords { get; set; }
    public Word WordOfTheDay { get; set; }
    public DateTime LastUpdated { get; set; }
}
