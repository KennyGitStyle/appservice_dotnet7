﻿using Microsoft.EntityFrameworkCore;

namespace Northwind.Console.HierarchyMapping;
public class HierarchyDb : DbContext
{
    public DbSet<Person>? People { get; set; }
    public DbSet<Student>? Students { get; set; }
    public DbSet<Employee>? Employees { get; set; }
    public HierarchyDb(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            // .UseTphMappingStrategy();
            // .UseTptMappingStrategy();
            .UseTpcMappingStrategy()
            .Property(p => p.Id)
            .HasDefaultValueSql("NEXT VALUE FOR [PersonIds]");
        modelBuilder.HasSequence<int>("PersonIds", builder =>
        {
            builder.StartsAt(4);
        });
        Student p1 = new() { Id = 1, Name = "Roman Roy", Subject = "History" };
        Employee p2 = new() { Id = 2, Name = "Kendell Roy", HireDate = new(year: 2014, month: 4, day: 2) };
        Employee p3 = new() { Id = 3, Name = "Siobhan Roy", HireDate = new(year: 2021, month: 9, day: 18) };
        modelBuilder.Entity<Student>().HasData(p1);
        modelBuilder.Entity<Employee>().HasData(p2, p3);
    }
    
}
