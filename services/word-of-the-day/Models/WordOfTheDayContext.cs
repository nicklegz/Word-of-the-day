using Microsoft.EntityFrameworkCore;

namespace word_of_the_day.Models
{
    public class WordOfTheDayContext : DbContext
    {
        public WordOfTheDayContext(DbContextOptions<WordOfTheDayContext> options) : base(options)
        {
        }
        public DbSet<Word> Words { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PreviouslyUsedWord> PreviouslyUsedWords { get; set; }
        public DbSet<LikedWord> LikedWords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>().ToTable("Word");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<PreviouslyUsedWord>().ToTable("PreviouslyUsedWord");
            modelBuilder.Entity<LikedWord>().ToTable("LikedWord");
        }
    }
}