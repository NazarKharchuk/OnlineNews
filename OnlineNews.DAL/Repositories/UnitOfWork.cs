using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.DAL.Entities;
using OnlineNews.DAL.Interfaces;
using OnlineNews.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace OnlineNews.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private NewsContext db;
        private NewsRepository news_repository;
        private TagRepository tag_repository;
        private RubricRepository rubric_repository;

        public UnitOfWork(DbContextOptions<NewsContext> options)
        {
            db = new NewsContext(options);
        }

        public INewsRepository<News> News
        {
            get
            {
                if (news_repository == null)
                    news_repository = new NewsRepository(db);
                return news_repository;
            }
        }

        public IRepository<Rubric> Rubrics
        {
            get
            {
                if (rubric_repository == null)
                    rubric_repository = new RubricRepository(db);
                return rubric_repository;
            }
        }

        public ITagRepository<Tag> Tags
        {
            get
            {
                if (tag_repository == null)
                    tag_repository = new TagRepository(db);
                return tag_repository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
