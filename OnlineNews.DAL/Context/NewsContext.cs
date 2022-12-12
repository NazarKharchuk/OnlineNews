using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.DAL.Entities;

namespace OnlineNews.DAL.Context
{
    public class NewsContext : DbContext
    {
        public DbSet<Rubric> Rubrics { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public NewsContext(DbContextOptions<NewsContext> options) 
            :base(options) 
        {
            Database.EnsureCreated();
        }
        /*public NewsContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=NewsDB;Trusted_Connection=True;");
        }*/
    }
}
