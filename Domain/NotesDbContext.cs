using JotBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JotBot.Domain
{

    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        public DbSet<NoteEntity> Notes { get; set; }
    }

}
