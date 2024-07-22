using AutoMapper;
using Library.Models.ForCreate;
using Library.Models.ViewModels;
using Library_Domain.Interfaces;
using Library_Domain.Modles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthersController : ControllerBase
    {
        private readonly IRepository<Auther> repository;
        private readonly IMapper mapper;

        public AuthersController(IRepository<Auther> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{autherId}", Name = "GetAuther")]
        public async Task<ActionResult<Auther>> GetAuther(int autherId)
        {
            var auther = await repository.GetByIdAsync(autherId);

            if (auther == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<AutherWithoutBooks>(auther));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Auther>>> GetAuthers(int pageNumber = 1, int pageSize = 10)
        {
            var (authers, paginationData) = await repository.GetAllAsync(pageNumber, pageSize);

            Response.Headers.Add("X-pagination", JsonSerializer.Serialize(paginationData));

            return Ok(mapper.Map<List<AutherWithoutBooks>>(authers));
        }

        [HttpPost]
        public async Task<ActionResult<Auther>> CreateAuther(Auther auther)
        {
            repository.AddAsync(auther);

            return CreatedAtRoute("GetAuther", new { autherId = repository.GetLastIDAsync() }, mapper.Map<AutherWithoutBooks>(auther));
        }

        [HttpPut("{autherId}")]
        public async Task<ActionResult<Auther>> UpdateAuther(int autherId, Auther newAuther)
        {
            var oldAuther = await repository.GetByIdAsync(autherId);

            if (oldAuther == null)
                return NotFound();

            oldAuther.Name = newAuther.Name;
            oldAuther.BirthDate = newAuther.BirthDate;
            oldAuther.Biography = newAuther.Biography;

            repository.Update();

            return NoContent();
        }

        [HttpPatch("{autherId}")]
        public async Task<ActionResult<Auther>> PatiallyUpdateAuther(int autherId, JsonPatchDocument<AutherForCreate> document)
        {
            var oldAuther = await repository.GetByIdAsync(autherId);

            if (oldAuther == null)
                return NotFound();

            var autherToPatch = new AutherForCreate()
            {
                Name = oldAuther.Name,
                BirthDate = oldAuther.BirthDate,
                Biography = oldAuther.Biography,
            };

            document.ApplyTo(autherToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            oldAuther.Name = autherToPatch.Name;
            oldAuther.BirthDate = autherToPatch.BirthDate;
            oldAuther.Biography = autherToPatch.Biography;

            repository.Update();

            return NoContent();
        }

        [HttpDelete("{autherId}")]
        public async Task<ActionResult<Auther>> DeleteAuther(int autherId)
        {
            var auther = await repository.GetByIdAsync(autherId);

            if (auther == null) return NotFound();

            repository.Delete(auther);

            return NoContent();
        }
    }
}
