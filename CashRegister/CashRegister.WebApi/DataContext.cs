using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Shared
{
    public class DataContext: DbContext
    {
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReceiptLine> ReceiptLines { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        { }
    }
}
