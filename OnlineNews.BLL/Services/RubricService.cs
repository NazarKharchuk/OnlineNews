using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.BLL.Interfaces;
using OnlineNews.BLL.DTO;
using OnlineNews.BLL.Infrastructure;
using OnlineNews.DAL.Interfaces;
using OnlineNews.DAL.Entities;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Ninject.Modules;
using OnlineNews.DAL.Context;
using Ninject;

namespace OnlineNews.BLL.Services
{
    public class RubricService : IService<RubricDTO>
    {
        IUnitOfWork DataBase { get; set; }

        public RubricService(IUnitOfWork uow)
        {
            DataBase = uow;
        }

        /*public RubricService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NewsContext>();

            var options = optionsBuilder
                    .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=NewsDB;Trusted_Connection=True;")
                    .Options;

            NinjectModule serviceModule = new ServiceModule(options);

            var kernel = new StandardKernel(serviceModule);

            DataBase = kernel.Get<IUnitOfWork>();
        }*/

        public IEnumerable<RubricDTO> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Rubric, RubricDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Rubric>, List<RubricDTO>>(DataBase.Rubrics.GetAll());
        }

        public RubricDTO Get(int id)
        {
            var rubric = DataBase.Rubrics.Get(id);
            if (rubric == null)
                throw new ValidationException("Rubric not found", "");

            return new RubricDTO { RubricId = rubric.RubricId, RubricName = rubric.RubricName };
        }

        public void Create(RubricDTO item)
        {
            if (item.RubricName == "")
                throw new ValidationException("Empty Rubric name", "");
            DataBase.Rubrics.Create(new Rubric { RubricName = item.RubricName });
            DataBase.Save();
        }

        public void Update(RubricDTO item)
        {
            Rubric rubric = DataBase.Rubrics.Get(item.RubricId);
            if (rubric == null)
                throw new ValidationException("Rubric not found", "");
            rubric.RubricName = item.RubricName;
            DataBase.Rubrics.Update(rubric);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.Rubrics.Delete(id);
            DataBase.Save();
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
