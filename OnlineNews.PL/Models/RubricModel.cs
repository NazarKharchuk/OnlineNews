using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineNews.PL.Models
{
    public class RubricModel
    {
        public int RubricId { get; set; }
        [Required(ErrorMessage = "Please enter rubric name.")] 
        [MaxLength(30, ErrorMessage = "The length of the rubric name must be less than 30 characters.")] 
        public string RubricName { get; set; }
    }
}
