using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace OnlineNews.DAL.Entities
{
    public class News
    {
        public int NewsId { get; set; }
        [Required] [MaxLength(50)] public string Title { get; set; }
        [Required] public string Content { get; set; }
        [Required] [MaxLength(30)] public string Author { get; set; }
        [Required] public DateTime Date { get; set; }

        public int RubricId { get; set; }
        public Rubric Rubric { get; set; }

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
