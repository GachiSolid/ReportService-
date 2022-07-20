﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReportingService.DAL.Models;

namespace ReportingService.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
