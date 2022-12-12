using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineNews.BLL.DTO
{
    public class NewsDTO
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public int RubricId { get; set; }
    }
}
