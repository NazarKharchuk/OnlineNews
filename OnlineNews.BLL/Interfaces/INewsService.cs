using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.BLL.DTO;

namespace OnlineNews.BLL.Interfaces
{
    public interface INewsService<NewsDTO> : IService<NewsDTO>
    {
        void AddTag(NewsDTO news, TagDTO tag);
        IEnumerable<TagDTO> GetTags(int id);
        IEnumerable<NewsDTO> FindByRubric(RubricDTO rubric);
        IEnumerable<NewsDTO> FindByTag(TagDTO tag);
        IEnumerable<NewsDTO> FindByAuthor(string author);
        IEnumerable<NewsDTO> FindByDate(DateTime start, DateTime finish);
    }
}
