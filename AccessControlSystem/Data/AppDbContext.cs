using AccessControlSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessControlSystem.Data;

public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<VehiclePass> VehiclePasses { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Employee
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CardNumber).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CardNumber).IsRequired().HasMaxLength(50);
            });
            
            // VehiclePass
            modelBuilder.Entity<VehiclePass>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.VehicleNumber).IsRequired().HasMaxLength(20);
                entity.Property(v => v.DriverName).IsRequired().HasMaxLength(200);
                entity.HasIndex(v => v.VehicleNumber);
                entity.HasIndex(v => v.EntryTime);
            });
            
            // Visitor
            modelBuilder.Entity<Visitor>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.FullName).IsRequired().HasMaxLength(200);
                entity.Property(v => v.Organization).IsRequired().HasMaxLength(200);
                entity.HasIndex(v => v.EntryTime);
            });
            
            // AccessLog
            modelBuilder.Entity<AccessLog>(entity =>
            {
                entity.HasKey(e => e.Id);
    
                // Конфигурация обычных свойств
                entity.Property(e => e.EntityType)
                    .IsRequired();
    
                entity.Property(e => e.EntityId)
                    .IsRequired(); // Это НЕ внешний ключ, просто int
    
                entity.Property(e => e.AccessTime)
                    .IsRequired();
    
                entity.Property(e => e.IsEntry)
                    .IsRequired();
    
                entity.Property(e => e.Reason)
                    .HasMaxLength(500);
    
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
    
                // Внешние ключи (nullable)
                entity.Property(e => e.EmployeeId)
                    .IsRequired(false);
    
                entity.Property(e => e.VehiclePassId)
                    .IsRequired(false);
    
                entity.Property(e => e.VisitorId)
                    .IsRequired(false);
    
                // Связи (внешние ключи)
                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
    
                entity.HasOne(e => e.VehiclePass)
                    .WithMany()
                    .HasForeignKey(e => e.VehiclePassId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
    
                entity.HasOne(e => e.Visitor)
                    .WithMany()
                    .HasForeignKey(e => e.VisitorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
            });
            
            // Seed данные для тестирования
            modelBuilder.Entity<Employee>().HasData(
                new Employee 
                { 
                    Id = 1, 
                    FullName = "Иванов Иван Иванович", 
                    CardNumber = "EMP001",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Employee 
                { 
                    Id = 2, 
                    FullName = "Петров Петр Петрович", 
                    CardNumber = "EMP002",
                    IsActive = false, // Неактивный!
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }