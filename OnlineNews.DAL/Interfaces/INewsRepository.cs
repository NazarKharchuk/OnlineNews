using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.DAL.Interfaces;
using OnlineNews.DAL.Entities;

namespace OnlineNews.DAL.Interfaces
{
    public interface INewsRepository<News> : IRepository<News>
    {
        void AddTag(News news, Tag tag);
    }
}
