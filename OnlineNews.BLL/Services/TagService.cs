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
    public class TagService : ITagService<TagDTO>
    {
        IUnitOfWork DataBase { get; set; }

        public TagService(IUnitOfWork uow)
        {
            DataBase = uow;
        }

        public IEnumerable<TagDTO> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Tag, TagDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Tag>, List<TagDTO>>(DataBase.Tags.GetAll());
        }

        public TagDTO Get(int id)
        {
            var tag = DataBase.Tags.Get(id);
            if (tag == null)
                throw new ValidationException("Tag not found", "");

            return new TagDTO { TagId = tag.TagId, TagName = tag.TagName };
        }

        public void Create(TagDTO item)
        {
            if (item.TagName == "")
                throw new ValidationException("Empty Tag name", "");
            DataBase.Tags.Create(new Tag { TagName = item.TagName });
            DataBase.Save();
        }

        public void Update(TagDTO item)
        {
            Tag tag = DataBase.Tags.Get(item.TagId);
            if (tag == null)
                throw new ValidationException("Tag not found", "");
            tag.TagName = item.TagName;
            DataBase.Tags.Update(tag);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.Tags.Delete(id);
            DataBase.Save();
        }

        public IEnumerable<NewsDTO> GetNews(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<News, NewsDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<News>, List<NewsDTO>>(DataBase.Tags.GetNews(id));
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
