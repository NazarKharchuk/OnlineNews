using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineNews.BLL.Services;
using OnlineNews.BLL.Interfaces;
using Ninject.Modules;
using OnlineNews.BLL.DTO;

namespace OnlineNews.PL.Util
{
    public class NewsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IService<RubricDTO>>().To<RubricService>();
            Bind<ITagService<TagDTO>>().To<TagService>();
            Bind<INewsService<NewsDTO>>().To<NewsService>();
        }
    }
}
