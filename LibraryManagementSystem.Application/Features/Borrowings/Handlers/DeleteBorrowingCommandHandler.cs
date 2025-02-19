using LibraryManagementSystem.Application.Features.Borrowings.Commands;
using LibraryManagementSystem.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Features.Borrowings.Handlers
{
    public class DeleteBorrowingCommandHandler : IRequestHandler<DeleteBorrowingCommand, bool>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly ILogger<DeleteBorrowingCommandHandler> _logger;

        public DeleteBorrowingCommandHandler(
            IBorrowingRepository borrowingRepository,
            ILogger<DeleteBorrowingCommandHandler> logger)
        {
            _borrowingRepository = borrowingRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteBorrowingCommand request, CancellationToken cancellationToken)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Borrowing with ID {request.Id} not found");

            if (!borrowing.IsReturned)
                throw new InvalidOperationException("Cannot delete active borrowing record");

            await _borrowingRepository.DeleteAsync(borrowing);
            return true;
        }
    }
}
