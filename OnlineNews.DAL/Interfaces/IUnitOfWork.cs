using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.DAL.Entities;

namespace OnlineNews.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Rubric> Rubrics { get; }
        INewsRepository<News> News { get; }
        ITagRepository<Tag> Tags { get; }
        void Save();
    }
}
