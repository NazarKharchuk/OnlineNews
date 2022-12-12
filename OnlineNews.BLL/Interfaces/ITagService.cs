using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.BLL.DTO;

namespace OnlineNews.BLL.Interfaces
{
    public interface ITagService<TagDTO> : IService<TagDTO>
    {
        IEnumerable<NewsDTO> GetNews(int id);
    }
}
