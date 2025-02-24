# Library Management System API

A comprehensive library management solution built with ASP.NET Core API, implementing modern architecture and design patterns.

## Features

### User Management
- Multiple role-based access control (Admin, Librarian, Author, User)
- Complete authentication system with JWT tokens
- Email verification
- Password management (reset, change, forgot password functionality)
- User profile management

### Book Management
- Complete CRUD operations for books
- Authors can create and manage their books
- Genre categorization
- Book availability tracking

### Library Operations
- Book borrowing system
- Review and rating system
- Book search and filtering capabilities
- Pagination for optimal performance

### Role-Based Features

#### Admin
- User role management (assign, update, delete roles)
- System administration
- Access to all operations

#### Librarian
- Book inventory management
- Handle borrowing operations
- User management

#### Author
- Create and manage books
- Update book information
- View book statistics

#### User
- Browse books
- Borrow books
- Add reviews and ratings
- View borrowing history

## Technical Stack

### Backend
- **Framework**: ASP.NET Core 8 API
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: JWT with ASP.NET Core Identity

### Architecture & Patterns
- **Architecture Pattern**: Clean Architecture
- **Design Patterns**:
  - CQRS (Command Query Responsibility Segregation)
  - MediatR for implementing CQRS
  - Repository Pattern
  - Unit of Work Pattern
  - Specification Pattern
- **Data Transfer**: DTOs with AutoMapper

## Security

- JWT token authentication
- Role-based authorization
- Email verification
- Password hashing
- Secure password reset flow
- Input validation

## Prerequisites

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or VS Code
- Email service configuration (SMTP)

## Installation & Setup

1. Clone the repository
```bash
git clone https://github.com/Mahmoud-Elaaser/LibraryManagementSystem.Api
```

2. Navigate to the project directory
```bash
cd LibraryManagementSystem
```

3. Update database connection string in `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your-Connection-String"
  }
}
```

4. Restore packages:
```bash
dotnet restore
```

5. Configure JWT & email settings in `appsettings.json`
```json
{
  "JWT": {
  "ValidAudience": "",
  "ValidIssuer": "",
  "Secret": "",
  "TokenValidityInMinutes": 180,
  "RefreshTokenValidityInDays": 7
  }
},
{
  "EmailSettings": {
    "SmtpServer": "smtp.example.com",
    "Port": 587,
    "Username": "your-email@example.com",
    "Password": "your-password"
  }
}
```

6. Run the application
```bash
dotnet run
```
<!--
## API Endpoints

### Authentication
- POST `/api/auth/register` - User registration
- POST `/api/auth/login` - User login
- POST `/api/auth/forgot-password` - Initiate password reset
- POST `/api/auth/reset-password` - Complete password reset
- POST `/api/auth/change-password` - Change password
- GET `/api/auth/confirm-email` - Email confirmation
- POST `/api/auth/assign-role` - Assign role to user
- PUT `/api/auth/update-role` - Update user role
- DELETE `/api/auth/delete-role` - Remove role from user

### Books
- GET `/api/books` - Get all books (with pagination)
- GET `/api/books/{id}` - Get specific book
- POST `/api/books` - Create new book (Author role)
- PUT `/api/books/{id}` - Update book
- DELETE `/api/books/{id}` - Delete book

### Authors
- GET `/api/authors` - Get all authors
- GET `/api/authors/{id}` - Get specific author
- GET `/api/authors/{id}/books` - Get author's books

### Borrowing
- POST `/api/borrowing` - Borrow a book
- PUT `/api/borrowing/{id}` - Return a book
- GET `/api/borrowing/user/{userId}` - Get user's borrowing history

### Reviews
- POST `/api/reviews` - Add book review
- GET `/api/reviews/book/{bookId}` - Get book reviews
- PUT `/api/reviews/{id}` - Update review
- DELETE `/api/reviews/{id}` - Delete review

### Genres
- GET `/api/genres` - Get all genres
- POST `/api/genres` - Create new genre
- PUT `/api/genres/{id}` - Update genre
- DELETE `/api/genres/{id}` - Delete genre




## 🧪 Testing

Run unit tests:
```bash
dotnet test
```
-->
## Future Improvements

- Implement caching
- Add book reservations
- Integrate payment system for fines
- Add book recommendations
- Implement real-time notifications
- Add reporting system
- Enhance search functionality with Elasticsearch

## Screenshot

Here is a full screenshot of the project:

![Screenshot](https://github.com/Mahmoud-Elaaser/LibraryManagementSystem.Api/blob/master/LibraryAPI.png)

