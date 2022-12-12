using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.DAL.Entities;
using OnlineNews.DAL.Interfaces;
using OnlineNews.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OnlineNews.DAL.Repositories
{
    public class NewsRepository : INewsRepository<News>
    {
        private NewsContext db;

        public NewsRepository(NewsContext context)
        {
            db = context;
        }

        public IEnumerable<News> GetAll()
        {
            return db.News;
        }

        public News Get(int id)
        {
            return db.News.Find(id);
        }

        public void Create(News news)
        {
            db.News.Add(news);
        }

        public void Update(News news)
        {
            db.Entry(news).State = EntityState.Modified;
        }

        public IEnumerable<News> Find(Func<News, Boolean> predicate)
        {
            return db.News.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            News news = db.News.Find(id);
            if (news != null)
                db.News.Remove(news);
        }

        public void AddTag(News news, Tag tag)
        {
            news.Tags.Add(tag);
        }
    }
}
