using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace OnlineNews.DAL.Entities
{
    public class Rubric
    {
        public int RubricId { get; set; }
        [Required] [MaxLength(30)] public string RubricName { get; set; }
    }
}
