using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace OnlineNews.DAL.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        [Required] [MaxLength(20)] public string TagName { get; set; }

        public ICollection<News> News { get; set; } = new List<News>();
    }
}
