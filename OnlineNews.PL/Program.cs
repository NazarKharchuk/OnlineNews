using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*using Ninject.Modules;
using OnlineNews.BLL.Infrastructure;
using OnlineNews.PL.Util;
using Microsoft.EntityFrameworkCore;
using OnlineNews.DAL.Context;
using Ninject;
using System.Web.Mvc;
using Ninject.Web.WebApi;*/

namespace OnlineNews.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*var optionsBuilder = new DbContextOptionsBuilder<NewsContext>();

            var options = optionsBuilder
                    .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=NewsDB;Trusted_Connection=True;")
                    .Options;

            NinjectModule serviceModule = new ServiceModule(options);
            NinjectModule newsModule = new NewsModule();

            var kernel = new StandardKernel(newsModule, serviceModule);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));*/

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
