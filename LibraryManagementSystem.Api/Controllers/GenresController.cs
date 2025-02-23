using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Genres.Commands;
using LibraryManagementSystem.Application.Features.Genres.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetAll()
        {
            var query = new GetGenresQuery();
            var genres = await _mediator.Send(query);
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDto>> GetById(int id)
        {
            try
            {
                var query = new GetGenreByIdQuery { Id = id };
                var genre = await _mediator.Send(query);
                return Ok(genre);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,Librarian,Author")]
        public async Task<ActionResult<GenreDto>> Create([FromBody] CreateGenreCommand command)
        {
            var genre = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = genre.Id }, genre);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenreDto>> Update(int id, [FromBody] UpdateGenreCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match ID in request body");
            }

            try
            {
                var genre = await _mediator.Send(command);
                return Ok(genre);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,Librarian,Author")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteGenreCommand { Id = id };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
