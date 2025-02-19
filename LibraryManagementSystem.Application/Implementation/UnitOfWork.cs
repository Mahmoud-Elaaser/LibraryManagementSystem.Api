//using LibraryManagementSystem.Application.Interfaces;
//using LibraryManagementSystem.Infrastructure.Context;
//using LibraryManagementSystem.Infrastructure.Repositories.Implementation;

//namespace LibraryManagementSystem.Application.Implementation
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        private readonly ApplicationDbContext _context;

//        public UnitOfWork(ApplicationDbContext context)
//        {
//            _context = context;
//            Books = new BookRepository(_context);
//            Authors = new AuthorRepository(_context);
//        }

//        public IBookRepository Books { get; }
//        public IAuthorRepository Authors { get; }

//        public async Task<int> CompleteAsync()
//        {
//            return await _context.SaveChangesAsync();
//        }
//    }

//}
