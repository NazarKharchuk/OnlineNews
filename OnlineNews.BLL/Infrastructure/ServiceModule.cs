using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Ninject.Modules;
using OnlineNews.DAL.Context;
using OnlineNews.DAL.Interfaces;
using OnlineNews.DAL.Repositories;

namespace OnlineNews.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private DbContextOptions<NewsContext> options;

        public ServiceModule(DbContextOptions<NewsContext> _options)
        {
            options = _options;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(options);
        }
    }
}
