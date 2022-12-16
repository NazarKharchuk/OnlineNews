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
    public class TagController : Controller
    {
        ITagService<TagDTO> tagService;
        public TagController(ITagService<TagDTO> serv)
        {
            tagService = serv;
        }

        /*public TagController()
        {
            NinjectModule newsModule = new NewsModule();

            var kernel = new StandardKernel(newsModule);

            tagService = kernel.Get<ITagService<TagDTO>>();
        }*/

        // GET: api/<TagController>
        [HttpGet]
        public IActionResult Get()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TagDTO, TagModel>()).CreateMapper();
            return new ObjectResult(mapper.Map<IEnumerable<TagDTO>, List<TagModel>>(tagService.GetAll()));
        }

        // GET api/<TagController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var tagItem = tagService.Get(id);

                return new ObjectResult(ItemFromDTO(tagItem));
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }

        }

        // POST api/<TagController>
        [HttpPost]
        public IActionResult Post([FromBody] TagModel item)
        {
            try
            {
                tagService.Create(DTOFromItem(item));

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        // PUT api/<TagController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TagModel item)
        {
            if (id != item.TagId)
            {
                return BadRequest();
            }

            try
            {
                tagService.Update(DTOFromItem(item));

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        // DELETE api/<TagController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var rubricItem = tagService.Get(id);
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }

            tagService.Delete(id);
            return NoContent();
        }

        /*
         get news
         */

        private static TagModel ItemFromDTO(TagDTO item) =>
            new TagModel
            {
                TagId = item.TagId,
                TagName = item.TagName
            };

        private static TagDTO DTOFromItem(TagModel item) =>
            new TagDTO
            {
                TagId = item.TagId,
                TagName = item.TagName
            };
    }
}
