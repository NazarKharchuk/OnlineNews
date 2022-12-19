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
    public class TestRubricService
    {
        private readonly RubricService rubricService;

        public TestRubricService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NewsContext>();

            var options = optionsBuilder
                    .UseInMemoryDatabase(databaseName: "InMemoryRubricDb")
                    .Options;

            UnitOfWork uow = new UnitOfWork(options);

            rubricService = new RubricService(uow);
        }

        [Fact]
        public void Create_AddNewRubric_NewRubricCreated()
        {
            RubricDTO rubric = new RubricDTO() { RubricName = "Rubric name #01" };

            rubricService.Create(rubric);

            var all_item = rubricService.GetAll();
            var item = all_item.FirstOrDefault(i => i.RubricName == "Rubric name #01");
            Assert.Equal("Rubric name #01", item.RubricName);
        }

        [Fact]
        public void Create_AddNewRubricWithEmptyName_ValidationExceptionReturned()
        {
            RubricDTO rubric = new RubricDTO() { RubricName = "" };

            try
            {
                rubricService.Create(rubric);

            }
            catch (ValidationException e)
            {
                Assert.Equal("Empty Rubric name", e.Message);
            }
        }

        [Fact]
        public void Get_GetByInvalidID_ValidationExceptionReturned()
        {
            try
            {
                rubricService.Get(20);

            }
            catch (ValidationException e)
            {
                Assert.Equal("Rubric not found", e.Message);
            }
        }

        [Fact]
        public void Update_UpdateInvalIdRubric_ValidationExceptionReturned()
        {
            RubricDTO rubric = new RubricDTO() { RubricId = 20, RubricName = "Tag name #20" };
            
            try
            {
                rubricService.Update(rubric);

            }
            catch (ValidationException e)
            {
                Assert.Equal("Rubric not found", e.Message);
            }

        }

        [Fact]
        public void Update_UpdateRubric_RubricUpdated()
        {
            rubricService.Create(new RubricDTO() { RubricName = "Rubric name" });
            var all_item = rubricService.GetAll();
            var item = all_item.FirstOrDefault(i => i.RubricName == "Rubric name");
            item.RubricName = "Other Rubric name";

            rubricService.Update(item);

            Assert.Equal("Other Rubric name", item.RubricName);
        }

        [Fact]
        public void Delete_DeleteRubric_RubricDeleted()
        {
            rubricService.Create(new RubricDTO() { RubricName = "For deleting" });
            var all_item = rubricService.GetAll();
            var item = all_item.FirstOrDefault(i => i.RubricName == "For deleting");

            rubricService.Delete(item.RubricId);

            Assert.NotNull(item);
            all_item = rubricService.GetAll();
            item = all_item.FirstOrDefault(i => i.RubricName == "For deleting");
            Assert.Null(item);
        }
    }
}
