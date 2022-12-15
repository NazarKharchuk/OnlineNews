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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineNews.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RubricController : ControllerBase
    {
        IService<RubricDTO> rubricService;
        /*public RubricController(IService<RubricDTO> serv)
        {
            rubricService = serv;
        }*/

        public RubricController()
        {
            NinjectModule newsModule = new NewsModule();

            var kernel = new StandardKernel(newsModule);

            rubricService = kernel.Get<IService<RubricDTO>>();
        }

        // GET: api/<RubricController>
        [HttpGet]
        public IActionResult Get()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RubricDTO, RubricModel>()).CreateMapper();
            return new ObjectResult(mapper.Map<IEnumerable<RubricDTO>, List<RubricModel>>(rubricService.GetAll()));
        }

        // GET api/<RubricController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var rubricItem = rubricService.Get(id);

                return new ObjectResult(ItemFromDTO(rubricItem));
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }

        }

        // POST api/<RubricController>
        [HttpPost]
        public IActionResult Post([FromBody] RubricModel item)
        {
            try
            {
                rubricService.Create(DTOFromItem(item));

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        // PUT api/<RubricController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RubricModel item)
        {
            if (id != item.RubricId)
            {
                return BadRequest();
            }

            try
            {
                rubricService.Update(DTOFromItem(item));

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        // DELETE api/<RubricController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var rubricItem = rubricService.Get(id);
            }
            catch (ValidationException ex)
            {
                return StatusCode(404, ex.Message);
            }

            rubricService.Delete(id);
            return NoContent();
        }

        private static RubricModel ItemFromDTO(RubricDTO item) =>
            new RubricModel
            {
                RubricId = item.RubricId,
                RubricName = item.RubricName
            };

        private static RubricDTO DTOFromItem(RubricModel item) =>
            new RubricDTO
            {
                RubricId = item.RubricId,
                RubricName = item.RubricName
            };
    }
}
