﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBModel : DbContext
    {
        public DBModel()
            : base("name=DBModel")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<StockOrder> StockOrders { get; set; }
        public virtual DbSet<OrderItems> OrderItems1 { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<BranchProduct> BranchProducts { get; set; }
        public virtual DbSet<SellProduct> SellProducts { get; set; }
    }
}
