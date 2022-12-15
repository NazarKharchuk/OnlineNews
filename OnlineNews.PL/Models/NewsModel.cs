using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineNews.PL.Models
{
    public class NewsModel
    {
        public int NewsId { get; set; }
        [Required(ErrorMessage = "Please enter title.")] 
        [MaxLength(50, ErrorMessage = "The length of the title must be less than 50 characters.")] 
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter content.")] 
        public string Content { get; set; }
        [Required(ErrorMessage = "Please enter author.")] 
        [MaxLength(30, ErrorMessage = "The length of the author name must be less than 30 characters.")] 
        public string Author { get; set; }
        [Required(ErrorMessage = "Please enter date.")] 
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Please enter rubric id.")] 
        public int RubricId { get; set; }
    }
}
