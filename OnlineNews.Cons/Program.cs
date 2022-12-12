using System;
using Microsoft.EntityFrameworkCore;
using OnlineNews.DAL.Repositories;
using OnlineNews.DAL.Context;
using OnlineNews.DAL.Entities;
using System.Linq;

namespace OnlineNews.Cons
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var optionsBuilder = new DbContextOptionsBuilder<NewsContext>();

            var options = optionsBuilder
                    .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=NewsDB;Trusted_Connection=True;")
                    .Options;

            UnitOfWork uow = new UnitOfWork(options);

             var news = uow.News.GetTags(1);
            foreach (var i in news)
            {
                Console.WriteLine("iD - {0}; Title - {1};", i.TagId, i.TagName);
            }
        }
    }
}
