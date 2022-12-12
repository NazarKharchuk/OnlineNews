using System;
using System.Collections.Generic;
using System.Text;
using Ninject.Modules;
using OnlineNews.DAL.Interfaces;
using OnlineNews.DAL.Repositories;

namespace OnlineNews.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;

        public ServiceModule(string connection)
        {
            connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
