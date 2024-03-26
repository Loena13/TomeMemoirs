using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomeMemoirs.Model;

namespace TomeMemoirs.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookUser> BookUsers { get; set; }

        //Connect Database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +
                "port=3306;" +
                "user=c_sharp;" +
                "password=Krijnisleider;" +
                "database=Db_TomeMemoir",
                ServerVersion.Parse("8.0.30")
            );
        }
    }
}
