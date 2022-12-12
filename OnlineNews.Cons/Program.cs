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

            /*using (NewsContext DbContect = new NewsContext(options))
            {
                //Rubric r = new Rubric() { RubricName = "RubricName1" };
                //DbContect.Rubrics.Add(r);
                //DbContect.SaveChanges();

                var rubrics = DbContect.Rubrics;
                foreach(var i in rubrics)
                {
                    Console.WriteLine("id - {0}; name - {1}", i.RubricId, i.RubricName);
                }
            }*/

            UnitOfWork uow = new UnitOfWork(options);

            /*Rubric r = uow.Rubrics.Find(t => t.RubricName == "RubricName1").First();
            Console.WriteLine(r.RubricId);

            News n = new News() { RubricId = r.RubricId, Title = "Title of news #1", Date = new DateTime(2017, 3, 25), Author = "The author of news #1", Content = "Content of news #1 written by author #1 in 2017." };
            uow.News.Create(n);
            Console.WriteLine(n.NewsId);

            Tag t = uow.Tags.Find(t => t.TagName == "Tag name #2").First();
            Console.WriteLine(t.TagId);

            uow.News.AddTag(n, t);

            uow.Save();

            var news = uow.News.GetAll();
            foreach (var i in news)
            {
                Console.WriteLine("id - {0}; title - {1}; date - {2}; author - {3}; content - {4}; rubric - {5}", i.NewsId, i.Title, i.Date, i.Author, i.Content, i.RubricId);

            }*/
            var news = uow.News.GetAll();
            foreach (var i in news)
            {
                Console.WriteLine("id - {0}; title - {1};", i.NewsId, i.Title);
                foreach (var t in i.Tags)
                {
                    Console.WriteLine("t id - {0}; t name - {1};", t.TagId, t.TagName);

                }
            }
        }
    }
}
