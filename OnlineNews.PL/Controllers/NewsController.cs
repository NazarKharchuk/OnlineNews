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
        [Route("api/news")]
        [HttpGet]
        public IActionResult Get()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, NewsModel>()).CreateMapper();
            return new ObjectResult(mapper.Map<IEnumerable<NewsDTO>, List<NewsModel>>(newsService.GetAll()));
        }

        // GET: api/<NewsController?author=value>
        [Route("api/news/author")]
        [HttpGet]
        public IActionResult Get([FromQuery] string author)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, NewsModel>()).CreateMapper();
            return new ObjectResult(mapper.Map<IEnumerable<NewsDTO>, List<NewsModel>>(newsService.FindByAuthor(author)));
        }

        // GET: api/<NewsController?datestart=value1,datefinish=value2>
        [Route("api/news/date")]
        [HttpGet]
        public IActionResult Get([FromQuery] DateTime datestart, [FromQuery] DateTime datefinish)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, NewsModel>()).CreateMapper();
            return new ObjectResult(mapper.Map<IEnumerable<NewsDTO>, List<NewsModel>>(newsService.FindByDate(datestart, datefinish)));
        }

        // GET: api/<NewsController?rubric=value>
        [Route("api/news/rubric")]
        [HttpGet]
        public IActionResult Get([FromQuery] RubricDTO rubric)
        {
            try
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, NewsModel>()).CreateMapper();
                return new ObjectResult(mapper.Map<IEnumerable<NewsDTO>,
                    List<NewsModel>>(newsService.FindByRubric(new RubricDTO() { RubricId = rubric.RubricId, RubricName = rubric.RubricName })));
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        // GET: api/<NewsController?tag=value>
        [Route("api/news/tag")]
        [HttpGet]
        public IActionResult Get([FromQuery] TagDTO tag)
        {
            try
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, NewsModel>()).CreateMapper();
                return new ObjectResult(mapper.Map<IEnumerable<NewsDTO>,
                    List<NewsModel>>(newsService.FindByTag(new TagDTO() { TagId = tag.TagId, TagName = tag.TagName })));
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        // GET api/<NewsController>/5
        [Route("api/news/{id}")]
        [HttpGet]
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
        [Route("api/news")]
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
        [Route("api/news/{id}")]
        [HttpPut]
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
        [Route("api/news/{id}")]
        [HttpDelete]
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

        // GET api/<NewsController>/5/tag
        [Route("api/news/{newsId}/tags")]
        [HttpGet]
        public IActionResult GetTagsByNews(int newsId)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TagDTO, TagModel>()).CreateMapper();
            return new ObjectResult(mapper.Map<IEnumerable<TagDTO>, List<TagModel>>(newsService.GetTags(newsId)));
        }

        // GET api/<NewsController>/5/tag
        [Route("api/news/{newsId}/tags")]
        [HttpPost]
        public IActionResult AddTag(int newsId, [FromBody] TagModel item)
        {
            try
            {
                var news = newsService.Get(newsId);

                newsService.AddTag(news, new TagDTO() { TagId = item.TagId, TagName = item.TagName });

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
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
