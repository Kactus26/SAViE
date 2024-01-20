using Microsoft.EntityFrameworkCore;
using SAViE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SAViE
{
    class DataContext : DbContext
    {
        public DbSet<Notes> NotesSq { get; set; }
        public DbSet<Topic> TopicSq { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = NotesData.db");
        }
    }
}
