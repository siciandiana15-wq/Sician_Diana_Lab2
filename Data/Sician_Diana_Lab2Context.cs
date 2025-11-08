using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sician_Diana_Lab2.Models;

namespace Sician_Diana_Lab2.Data
{
    public class Sician_Diana_Lab2Context : DbContext
    {
        public Sician_Diana_Lab2Context (DbContextOptions<Sician_Diana_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Sician_Diana_Lab2.Models.Book> Book { get; set; } = default!;
        public DbSet<Sician_Diana_Lab2.Models.Publisher> Publisher { get; set; } = default!;
        public DbSet<Sician_Diana_Lab2.Models.Author> Author { get; set; } = default!;
        public DbSet<Sician_Diana_Lab2.Models.Category> Category { get; set; } = default!;
        public DbSet<Sician_Diana_Lab2.Models.Member> Member { get; set; } = default!;
        public DbSet<Sician_Diana_Lab2.Models.Borrowing> Borrowing { get; set; } = default!;
    }
}
