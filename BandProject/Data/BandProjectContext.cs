using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BandProject.Models;

namespace BandProject.Data
{
    public class BandProjectContext : DbContext
    {
        public BandProjectContext (DbContextOptions<BandProjectContext> options): base(options)
        {
        }
        public virtual DbSet<Tour> Tours { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set;}
    }
}
