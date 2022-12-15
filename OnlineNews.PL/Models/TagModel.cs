using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineNews.PL.Models
{
    public class TagModel
    {
        public int TagId { get; set; }
        [Required(ErrorMessage = "Please enter tag name.")] 
        [MaxLength(20, ErrorMessage = "The length of the tag name must be less than 20 characters.")] 
        public string TagName { get; set; }
    }
}
