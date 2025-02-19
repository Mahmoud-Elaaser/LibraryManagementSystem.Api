using LibraryManagementSystem.Application.DTOs;
using LibraryManagementSystem.Application.Features.Borrowings.Commands;
using LibraryManagementSystem.Application.Features.Borrowings.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BorrowingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetAll([FromQuery] bool? activeOnly, [FromQuery] bool? overdueOnly)
        {
            var query = new GetBorrowingsQuery
            {
                ActiveOnly = activeOnly,
                OverdueOnly = overdueOnly
            };
            var borrowings = await _mediator.Send(query);
            return Ok(borrowings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowingDto>> GetById(int id)
        {
            try
            {
                var query = new GetBorrowingByIdQuery { Id = id };
                var borrowing = await _mediator.Send(query);
                return Ok(borrowing);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetByUser(int userId, [FromQuery] bool? activeOnly)
        {
            var query = new GetUserBorrowingsQuery
            {
                UserId = userId,
                ActiveOnly = activeOnly
            };
            var borrowings = await _mediator.Send(query);
            return Ok(borrowings);
        }

        [HttpPost]
        public async Task<ActionResult<BorrowingDto>> Create([FromBody] CreateBorrowingCommand command)
        {
            try
            {
                var borrowing = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = borrowing.Id }, borrowing);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<BorrowingDto>> Update(int id, [FromBody] UpdateBorrowingCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match ID in request body");
            }

            try
            {
                var borrowing = await _mediator.Send(command);
                return Ok(borrowing);
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

        [HttpPost("{id}/return")]
        public async Task<ActionResult<BorrowingDto>> ReturnBook(int id)
        {
            try
            {
                var command = new ReturnBookCommand
                {
                    Id = id,
                    ReturnDate = DateTime.UtcNow
                };
                var borrowing = await _mediator.Send(command);
                return Ok(borrowing);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteBorrowingCommand { Id = id };
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
