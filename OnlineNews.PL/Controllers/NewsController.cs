using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineNews.BLL.DTO;
using OnlineNews.BLL.Services;
using OnlineNews.BLL.Interfaces;
using OnlineNews.PL.Models;
using AutoMapper;

using Ninject.Modules;
using Ninject;
using OnlineNews.PL.Util;
using OnlineNews.BLL.Infrastructure;
using System.Net;

namespace OnlineNews.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : Controller
    {
        INewsService<NewsDTO> newsService;
        /*public NewsController(INewsService<NewsDTO> serv)
        {
            newsService = serv;
        }*/

        public NewsController()
        {
            NinjectModule newsModule = new NewsModule();

            var kernel = new StandardKernel(newsModule);

            newsService = kernel.Get<INewsService<NewsDTO>>();
        }

        // GET: api/<NewsController>
        [HttpGet]
        public IActionResult Get()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, NewsModel>()).CreateMapper();
            return new ObjectResult(mapper.Map<IEnumerable<NewsDTO>, List<NewsModel>>(newsService.GetAll()));
        }

        // GET api/<NewsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var newsItem = newsService.Get(id);

                return new ObjectResult(ItemFromDTO(newsItem));
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }

        }

        // POST api/<NewsController>
        [HttpPost]
        public IActionResult Post([FromBody] NewsModel item)
        {
            try
            {
                newsService.Create(DTOFromItem(item));

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        // PUT api/<NewsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] NewsModel item)
        {
            if (id != item.RubricId)
            {
                return BadRequest();
            }

            try
            {
                newsService.Update(DTOFromItem(item));

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        // DELETE api/<NewsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var newsItem = newsService.Get(id);
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }

            newsService.Delete(id);
            return NoContent();
        }

        private static NewsModel ItemFromDTO(NewsDTO item) =>
            new NewsModel
            {
                NewsId = item.NewsId,
                Title = item.Title,
                Content = item.Content,
                Author = item.Author,
                Date = item.Date,
                RubricId = item.RubricId
            };

        private static NewsDTO DTOFromItem(NewsModel item) =>
            new NewsDTO
            {
                NewsId = item.NewsId,
                Title = item.Title,
                Content = item.Content,
                Author = item.Author,
                Date = item.Date,
                RubricId = item.RubricId
            };
    }
}
