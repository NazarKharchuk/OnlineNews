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
    public class TagRepository : ITagRepository<Tag>
    {
        private NewsContext db;

        public TagRepository(NewsContext context)
        {
            db = context;
        }

        public IEnumerable<Tag> GetAll()
        {
            return db.Tags;
        }

        public Tag Get(int id)
        {
            return db.Tags.Find(id);
        }

        public void Create(Tag tag)
        {
            db.Tags.Add(tag);
        }

        public void Update(Tag tag)
        {
            db.Entry(tag).State = EntityState.Modified;
        }

        public IEnumerable<Tag> Find(Func<Tag, Boolean> predicate)
        {
            return db.Tags.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Tag tag = db.Tags.Find(id);
            if (tag != null)
                db.Tags.Remove(tag);
        }

        public IEnumerable<News> GetNews(int id)
        {
            Tag tag = db.Tags.Include(n => n.News).Where(t => t.TagId == id).FirstOrDefault();

            IEnumerable<News> news = tag.News;
            
            return news;
        }
    }
}
