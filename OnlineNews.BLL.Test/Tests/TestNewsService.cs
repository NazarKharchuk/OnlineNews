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
using System.Linq;
using OnlineNews.BLL.Infrastructure;

namespace OnlineNews.BLL.Test.Tests
{
    public class TestNewsService
    {
        private readonly TagService tagService;
        private readonly NewsService newsService;
        private readonly RubricService rubricService;

        public TestNewsService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NewsContext>();

            var options = optionsBuilder
                    .UseInMemoryDatabase(databaseName: "InMemoryNewsDb")
                    .Options;

            UnitOfWork uow = new UnitOfWork(options);

            tagService = new TagService(uow);
            newsService = new NewsService(uow);
            rubricService = new RubricService(uow);
        }

        [Fact]
        public void Create_AddNewNews_NewNewsCreated()
        {
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name #1" });
            var all_rubrics = rubricService.GetAll();
            var rubric = all_rubrics.FirstOrDefault(i => i.RubricName == "Rubric name #1");
            NewsDTO news = new NewsDTO() { Title = "News title 1", Author = "Author 1", Content = "Content 1", 
                Date = new DateTime(2022, 9, 23), RubricId = rubric.RubricId };

            newsService.Create(news);

            var all_news = newsService.GetAll();
            var added_news = all_news.FirstOrDefault(i => i.Title == "News title 1");
            Assert.Equal("News title 1", added_news.Title);
            Assert.Equal("Author 1", added_news.Author);
            Assert.Equal(new DateTime(2022, 9, 23), added_news.Date);
            Assert.Equal("Content 1", added_news.Content);
            Assert.Equal(rubric.RubricId, added_news.RubricId);
        }

        [Fact]
        public void Create_AddNews_NewsCreated()
        {
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name #1" });
            var all_rubrics = rubricService.GetAll();
            var rubric = all_rubrics.FirstOrDefault(i => i.RubricName == "Rubric name #1");
            NewsDTO news = new NewsDTO()
            {
                Title = "News title 1",
                Author = "Author 1",
                Content = "Content 1",
                Date = new DateTime(2022, 9, 23),
                RubricId = rubric.RubricId
            };

            newsService.Create(news);

            var all_news = newsService.GetAll();
            var added_news = all_news.FirstOrDefault(i => i.Title == "News title 1");
            Assert.Equal("News title 1", added_news.Title);
            Assert.Equal("Author 1", added_news.Author);
            Assert.Equal(new DateTime(2022, 9, 23), added_news.Date);
            Assert.Equal("Content 1", added_news.Content);
            Assert.Equal(rubric.RubricId, added_news.RubricId);
        }

        [Fact]
        public void Update_UpdateNews_NewsUpdated()
        {
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name #2" });
            var all_rubrics = rubricService.GetAll();
            var rubric = all_rubrics.FirstOrDefault(i => i.RubricName == "Rubric name #2");
            NewsDTO news = new NewsDTO()
            {
                Title = "News title 2",
                Author = "Author 2",
                Content = "Content 2",
                Date = new DateTime(2022, 9, 24),
                RubricId = rubric.RubricId
            };
            newsService.Create(news);
            var all_news = newsService.GetAll();
            news = all_news.FirstOrDefault(i => i.Title == "News title 2");
            news.Content = "Other content";

            newsService.Update(news);

            all_news = newsService.GetAll();
            news = all_news.FirstOrDefault(i => i.Title == "News title 2");
            Assert.Equal("News title 2", news.Title);
            Assert.Equal("Author 2", news.Author);
            Assert.Equal(new DateTime(2022, 9, 24), news.Date);
            Assert.Equal("Other content", news.Content);
            Assert.Equal(rubric.RubricId, news.RubricId);
        }

        [Fact]
        public void Get_GetByInvalidID_ValidationExceptionReturned()
        {
            try
            {
                newsService.Get(20);

            }
            catch (ValidationException e)
            {
                Assert.Equal("News not found", e.Message);
            }
        }

        [Fact]
        public void Update_UpdateInvalidNews_ValidationExceptionReturned()
        {
            NewsDTO news = new NewsDTO()
            {
                NewsId = 14,
                Title = "News title 2",
                Author = "Author 2",
                Content = "Content 2",
                Date = new DateTime(2022, 9, 24)
            };

            try
            {
                newsService.Update(news);

            }
            catch (ValidationException e)
            {
                Assert.Equal("Invalid rubric", e.Message);
            }

        }

        [Fact]
        public void Delete_DeleteNews_NewsDeleted()
        {
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name #3" });
            var all_rubrics = rubricService.GetAll();
            var rubric = all_rubrics.FirstOrDefault(i => i.RubricName == "Rubric name #3");
            NewsDTO news = new NewsDTO()
            {
                Title = "For deleting",
                Author = "Author 3",
                Content = "Content 3",
                Date = new DateTime(2022, 9, 25),
                RubricId = rubric.RubricId
            };
            newsService.Create(news);
            var all_news = newsService.GetAll();
            news = all_news.FirstOrDefault(i => i.Title == "For deleting");

            newsService.Delete(news.NewsId);

            Assert.NotNull(news);
            all_news = newsService.GetAll();
            news = all_news.FirstOrDefault(i => i.Title == "For deleting"); ;
            Assert.Null(news);
        }

        [Fact]
        public void GetTags_GetTagsWithIdNews_TagsReturned()
        {
            tagService.Create(new TagDTO() { TagName = "Tag name 7" });
            var all_tags = tagService.GetAll();
            var tag = all_tags.FirstOrDefault(i => i.TagName == "Tag name 7");
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name 7" });
            var all_rubrics = rubricService.GetAll();
            var rubric = all_rubrics.FirstOrDefault(i => i.RubricName == "Rubric name 7");
            newsService.Create(new NewsDTO() { Title = "News title 7", Author = "Author 7", Content = "Content 7", Date = new DateTime(2022, 9, 27), RubricId = rubric.RubricId });
            var all_news = newsService.GetAll();
            var news = all_news.FirstOrDefault(i => i.Title == "News title 7");
            newsService.AddTag(news, tag);

            var tags_by_news = newsService.GetTags(news.NewsId);

            Assert.Single(tags_by_news);
            Assert.Equal("Tag name 7", tags_by_news.First().TagName);
        }

        [Fact]
        public void FindByRubric_FindNewsByRubric_NewsReturned()
        {
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name #4" });
            var all_rubrics = rubricService.GetAll();
            var rubric = all_rubrics.FirstOrDefault(i => i.RubricName == "Rubric name #4");
            NewsDTO news = new NewsDTO()
            {
                Title = "News title 4",
                Author = "Author 4",
                Content = "Content 4",
                Date = new DateTime(2022, 9, 24),
                RubricId = rubric.RubricId
            };
            newsService.Create(news);

            var news_by_rubric = newsService.FindByRubric(rubric);

            news = news_by_rubric.FirstOrDefault(i => i.Title == "News title 4");
            Assert.Equal("News title 4", news.Title);
            Assert.Equal("Author 4", news.Author);
            Assert.Equal(new DateTime(2022, 9, 24), news.Date);
            Assert.Equal("Content 4", news.Content);
            Assert.Equal(rubric.RubricId, news.RubricId);
        }

        [Fact]
        public void FindByTag_FindNewsByTag_NewsReturned()
        {
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name #5" });
            var all_rubrics = rubricService.GetAll();
            var rubric = all_rubrics.FirstOrDefault(i => i.RubricName == "Rubric name #5");
            NewsDTO news = new NewsDTO()
            {
                Title = "News title 5",
                Author = "Author 5",
                Content = "Content 5",
                Date = new DateTime(2022, 9, 25),
                RubricId = rubric.RubricId
            };
            newsService.Create(news);
            tagService.Create(new TagDTO() { TagName = "Tag name 5" });
            var all_tags = tagService.GetAll();
            var tag = all_tags.FirstOrDefault(i => i.TagName == "Tag name 5");
            var all_news = newsService.GetAll();
            news = all_news.FirstOrDefault(i => i.Title == "News title 5");
            newsService.AddTag(news, tag);

            var news_by_tag = newsService.FindByTag(tag);

            news = news_by_tag.FirstOrDefault(i => i.Title == "News title 5");
            Assert.Equal("News title 5", news.Title);
            Assert.Equal("Author 5", news.Author);
            Assert.Equal(new DateTime(2022, 9, 25), news.Date);
            Assert.Equal("Content 5", news.Content);
            Assert.Equal(rubric.RubricId, news.RubricId);
        }

        [Fact]
        public void FindByAuthor_FindNewsByAuthor_NewsReturned()
        {
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name #8" });
            var all_rubrics = rubricService.GetAll();
            var rubric = all_rubrics.FirstOrDefault(i => i.RubricName == "Rubric name #8");
            newsService.Create(new NewsDTO()
            {
                Title = "News title 8",
                Author = "First author",
                Content = "Content 8",
                Date = new DateTime(2022, 9, 28),
                RubricId = rubric.RubricId
            });
            newsService.Create(new NewsDTO()
            {
                Title = "News title 9",
                Author = "Second author",
                Content = "Content 9",
                Date = new DateTime(2022, 9, 30),
                RubricId = rubric.RubricId
            });

            var news_by_author = newsService.FindByAuthor("Second author");

            Assert.Single(news_by_author);
            var news = news_by_author.FirstOrDefault(i => i.Title == "News title 9");
            Assert.Equal("News title 9", news.Title);
            Assert.Equal("Second author", news.Author);
            Assert.Equal(new DateTime(2022, 9, 30), news.Date);
            Assert.Equal("Content 9", news.Content);
            Assert.Equal(rubric.RubricId, news.RubricId);
        }

        [Fact]
        public void FindByDate_FindNewsByDate_NewsReturned()
        {
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name #10" });
            var all_rubrics = rubricService.GetAll();
            var rubric = all_rubrics.FirstOrDefault(i => i.RubricName == "Rubric name #10");
            newsService.Create(new NewsDTO()
            {
                Title = "News title 10",
                Author = "First author",
                Content = "Content 10",
                Date = new DateTime(2022, 9, 5),
                RubricId = rubric.RubricId
            });
            newsService.Create(new NewsDTO()
            {
                Title = "News title 11",
                Author = "Second author",
                Content = "Content 11",
                Date = new DateTime(2022, 9, 15),
                RubricId = rubric.RubricId
            });

            var news_by_date = newsService.FindByDate(new DateTime(2022, 9, 1), new DateTime(2022, 9, 10));

            Assert.Single(news_by_date);
            var news = news_by_date.FirstOrDefault(i => i.Title == "News title 10");
            Assert.Equal("News title 10", news.Title);
            Assert.Equal("First author", news.Author);
            Assert.Equal(new DateTime(2022, 9, 5), news.Date);
            Assert.Equal("Content 10", news.Content);
            Assert.Equal(rubric.RubricId, news.RubricId);
        }
    }
}
