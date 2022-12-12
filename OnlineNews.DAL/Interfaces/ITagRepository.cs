using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.DAL.Interfaces;
using OnlineNews.DAL.Entities;

namespace OnlineNews.DAL.Interfaces
{
    public interface ITagRepository<Tag> : IRepository<Tag>
    {
        IEnumerable<News> GetNews(int id);
    }
}
