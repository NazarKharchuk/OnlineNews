using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.BLL.Interfaces;
using OnlineNews.BLL.DTO;
using OnlineNews.BLL.Infrastructure;
using OnlineNews.DAL.Interfaces;
using OnlineNews.DAL.Entities;
using AutoMapper;

namespace OnlineNews.BLL.Services
{
    public class NewsService : INewsService<NewsDTO>
    {
        IUnitOfWork DataBase { get; set; }

        public NewsService(IUnitOfWork uow)
        {
            DataBase = uow;
        }

        public IEnumerable<NewsDTO> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<News, NewsDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<News>, List<NewsDTO>>(DataBase.News.GetAll());
        }

        public NewsDTO Get(int id)
        {
            var news = DataBase.News.Get(id);
            if (news == null)
                throw new ValidationException("News not found", "");

            return new NewsDTO { NewsId = news.NewsId, Title = news.Title, Content = news.Content, Author = news.Author, Date = news.Date, RubricId = news.RubricId };
        }

        public void Create(NewsDTO item)
        {
            if (item.Title == "")
                throw new ValidationException("Empty News Title", "");
            if (item.Content == "")
                throw new ValidationException("Empty News Content", "");
            if (item.Author == "")
                throw new ValidationException("Empty News Author", "");

            DataBase.News.Create(new News { Title = item.Title, Content = item.Content, Author = item.Author, Date = item.Date});
            DataBase.Save();
        }

        public void Update(NewsDTO item)
        {
            News news = DataBase.News.Get(item.NewsId);
            if (news == null)
                throw new ValidationException("News not found", "");
            news.Title = item.Title;
            news.Content = item.Content;
            news.Author = item.Author;
            news.Date = item.Date;
            news.RubricId = item.RubricId;
            DataBase.News.Update(news);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.News.Delete(id);
            DataBase.Save();
        }

        public IEnumerable<TagDTO> GetTags(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Tag, TagDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Tag>, List<TagDTO>>(DataBase.News.GetTags(id));
        }

        public void AddTag(NewsDTO news, TagDTO tag)
        {
            News n = DataBase.News.Get(news.NewsId);
            if (n == null)
                throw new ValidationException("News not found", "");
            var t = DataBase.Tags.Get(tag.TagId);
            if (t == null)
                throw new ValidationException("Tag not found", "");
            DataBase.News.AddTag(n, t);
            DataBase.Save();
        }

        public IEnumerable<NewsDTO> FindByRubric(RubricDTO rubric)
        {
            Rubric r = DataBase.Rubrics.Get(rubric.RubricId);
            if (r == null)
                throw new ValidationException("Rubric not found", "");

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<News, NewsDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<News>, List<NewsDTO>>(DataBase.News.Find(n => n.RubricId == r.RubricId));
        }

        public IEnumerable<NewsDTO> FindByTag(TagDTO tag)
        {
            Tag t = DataBase.Tags.Get(tag.TagId);
            if (t == null)
                throw new ValidationException("Tag not found", "");

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<News, NewsDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<News>, List<NewsDTO>>(DataBase.Tags.GetNews(t.TagId));
        }

        public IEnumerable<NewsDTO> FindByAuthor(string author)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<News, NewsDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<News>, List<NewsDTO>>(DataBase.News.Find(n => n.Author == author));
        }

        public IEnumerable<NewsDTO> FindByDate(DateTime start, DateTime finish)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<News, NewsDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<News>, List<NewsDTO>>(DataBase.News.Find(n => n.Date > start && n.Date < finish));
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
