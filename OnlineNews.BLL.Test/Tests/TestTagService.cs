using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using OnlineNews.BLL.Services;
using OnlineNews.DAL.Context;
using OnlineNews.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using OnlineNews.DAL.Entities;
using OnlineNews.BLL.DTO;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OnlineNews.BLL.Infrastructure;
using System.Linq;

namespace OnlineNews.BLL.Test.Tests
{
    public class TestTagService
    {
        private readonly TagService tagService;
        private readonly NewsService newsService;
        private readonly RubricService rubricService;

        public TestTagService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NewsContext>();

            var options = optionsBuilder
                    .UseInMemoryDatabase(databaseName: "InMemoryTagDb")
                    .Options;

            UnitOfWork uow = new UnitOfWork(options);

            tagService = new TagService(uow);
            newsService = new NewsService(uow);
            rubricService = new RubricService(uow);
        }

        [Fact]
        public void Create_AddNewTag_NewTagCreated()
        {
            TagDTO tag = new TagDTO() { TagName = "Tag name #01" };

            tagService.Create(tag);

            var all_item = tagService.GetAll();
            var item = all_item.FirstOrDefault(i => i.TagName == "Tag name #01");
            Assert.Equal("Tag name #01", item.TagName);
        }

        [Fact]
        public void Create_AddNewTagWithEmptyName_ValidationExceptionReturned()
        {
            TagDTO tag = new TagDTO() { TagName = "" };

            try
            {
                tagService.Create(tag);

            }
            catch (ValidationException e)
            {
                Assert.Equal("Empty Tag name", e.Message);
            }
        }

        [Fact]
        public void Get_GetByInvalidID_ValidationExceptionReturned()
        {
            try
            {
                tagService.Get(20);

            }
            catch (ValidationException e)
            {
                Assert.Equal("Tag not found", e.Message);
            }
        }

        [Fact]
        public void Update_UpdateInvalidTag_ValidationExceptionReturned()
        {
            TagDTO tag = new TagDTO() { TagId = 20, TagName = "Tag name #20" };
            
            try
            {
                tagService.Update(tag);

            }
            catch (ValidationException e)
            {
                Assert.Equal("Tag not found", e.Message);
            }

        }

        [Fact]
        public void Update_UpdateTag_TagUpdated()
        {
            tagService.Create(new TagDTO() { TagName = "Tag name" });
            var all_item = tagService.GetAll();
            var item = all_item.FirstOrDefault(i => i.TagName == "Tag name");
            item.TagName = "Other tag name";

            tagService.Update(item);

            Assert.Equal("Other tag name", item.TagName);
        }

        [Fact]
        public void Delete_DeleteTag_TagDeleted()
        {
            tagService.Create(new TagDTO() { TagName = "For deleting" });
            var all_item = tagService.GetAll();
            var item = all_item.FirstOrDefault(i => i.TagName == "For deleting");

            tagService.Delete(item.TagId);

            Assert.NotNull(item);
            all_item = tagService.GetAll();
            item = all_item.FirstOrDefault(i => i.TagName == "For deleting");
            Assert.Null(item);
        }

        [Fact]
        public void GetNews_GetNewsWithIdTag_NewsReturned()
        {
            tagService.Create(new TagDTO() { TagName = "Tag name" });
            var all_tags = tagService.GetAll();
            var tag = all_tags.FirstOrDefault(i => i.TagName == "Tag name");
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name" });
            var all_rubrics = rubricService.GetAll();
            var rubric = all_rubrics.FirstOrDefault(i => i.RubricName == "Rubric name");
            newsService.Create(new NewsDTO() { Title = "News title", Author = "Author", Content = "Content", Date = new DateTime(2022, 9, 23), RubricId = rubric.RubricId });
            var all_news = newsService.GetAll();
            var news = all_news.FirstOrDefault(i => i.Title == "News title");
            newsService.AddTag(news, tag);

            var news_by_tag = tagService.GetNews(tag.TagId);

            Assert.Single(news_by_tag);
            Assert.Equal("News title", news_by_tag.First().Title);
            Assert.Equal(rubric.RubricId, news_by_tag.First().RubricId);
        }
    }
}
